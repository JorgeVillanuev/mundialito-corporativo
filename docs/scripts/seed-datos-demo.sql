-- ============================================================================
-- seed-datos-demo.sql
--
-- Script de USO LOCAL, fuera del alcance evaluado: agrega datos EXTRA sobre el
-- seed oficial (DataSeeder.cs, que sigue siendo el único seed que corre
-- automaticamente con la app) para poder demostrar filtros, ordenamiento y
-- paginacion con volumen real en la entrevista. No lo ejecuta la aplicacion,
-- se corre a mano contra la base ya migrada/sembrada.
--
-- Que agrega:
--   - 20 equipos nuevos (24 en total con los 4 originales), con CiudadOrigen
--     variada (y repetida a proposito, para poder probar el filtro exacto).
--   - 8 jugadores por equipo nuevo (para que "GET /equipos/{id}/jugadores"
--     tenga de sobra para paginar con un pageSize chico).
--   - 80 partidos nuevos entre equipos al azar, con fechas repartidas entre
--     -90 y +59 dias desde hoy (para poder probar fechaDesde/fechaHasta), y
--     Estado Jugado/Programado segun si la fecha ya paso.
--   - Goles de partido para los partidos Jugado con marcador > 0, repartidos
--     entre jugadores al azar de cada equipo (para que "GET /goleadores"
--     tenga variedad real al ordenar por cantidadGoles).
--
-- Como correrlo (con la Api ya migrada/corriendo al menos una vez):
--   sqlcmd -S localhost,1433 -U sa -P Mundialito2026! -d Mundialito -i scripts/seed-datos-demo.sql
-- o pegando el contenido en la extension mssql de VS Code conectada a la BD.
--
-- Es seguro correrlo varias veces: cada corrida agrega equipos NUEVOS con
-- nombres distintos (sufijo de fecha/hora en el nombre), nunca duplica ni
-- toca los equipos/partidos que ya existian.
-- ============================================================================

SET NOCOUNT ON;

DECLARE @Sufijo NVARCHAR(20) = FORMAT(SYSUTCDATETIME(), 'MMddHHmmss');

------------------------------------------------------------------------------
-- 1. Equipos nuevos
------------------------------------------------------------------------------
DECLARE @EquiposNuevos TABLE (Id UNIQUEIDENTIFIER, Nombre NVARCHAR(100), Ciudad NVARCHAR(150));

INSERT INTO @EquiposNuevos (Id, Nombre, Ciudad)
VALUES
    (NEWID(), 'Condores del Sur '      + @Sufijo, 'San Salvador'),
    (NEWID(), 'Piratas de Sonsonate '  + @Sufijo, 'Sonsonate'),
    (NEWID(), 'Titanes de Ahuachapan ' + @Sufijo, 'Ahuachapan'),
    (NEWID(), 'Marineros de La Union ' + @Sufijo, 'La Union'),
    (NEWID(), 'Lobos de Chalatenango ' + @Sufijo, 'Chalatenango'),
    (NEWID(), 'Panteras de Cuscatlan ' + @Sufijo, 'Cuscatlan'),
    (NEWID(), 'Guerreros de La Paz '   + @Sufijo, 'La Paz'),
    (NEWID(), 'Toros de Usulutan '     + @Sufijo, 'Usulutan'),
    (NEWID(), 'Halcones de Morazan '   + @Sufijo, 'Morazan'),
    (NEWID(), 'Jaguares de Cabanas '   + @Sufijo, 'Cabanas'),
    (NEWID(), 'Escorpiones del Norte ' + @Sufijo, 'Chalatenango'),
    (NEWID(), 'Delfines Costeros '     + @Sufijo, 'La Libertad'),
    (NEWID(), 'Vikingos de Santa Ana ' + @Sufijo, 'Santa Ana'),
    (NEWID(), 'Centauros de San Vicente ' + @Sufijo, 'San Vicente'),
    (NEWID(), 'Gladiadores del Este '  + @Sufijo, 'San Miguel'),
    (NEWID(), 'Fenix de San Salvador ' + @Sufijo, 'San Salvador'),
    (NEWID(), 'Cobras de Santa Ana '   + @Sufijo, 'Santa Ana'),
    (NEWID(), 'Rayos del Pacifico '    + @Sufijo, 'La Libertad'),
    (NEWID(), 'Espartanos FC '         + @Sufijo, 'San Miguel'),
    (NEWID(), 'Cometas de Cuscatlan '  + @Sufijo, 'Cuscatlan');

INSERT INTO Equipos (Id, Nombre, CiudadOrigen, CreadoEn)
SELECT Id, Nombre, Ciudad, SYSUTCDATETIME()
FROM @EquiposNuevos;

------------------------------------------------------------------------------
-- 2. Jugadores: 8 por cada equipo nuevo (1 Arquero=0, 3 Defensor=1,
--    2 Mediocampista=2, 2 Delantero=3 -- formacion tipica).
------------------------------------------------------------------------------
DECLARE @Posiciones TABLE (Num INT, Posicion INT);
INSERT INTO @Posiciones (Num, Posicion) VALUES
    (1, 0), (2, 1), (3, 1), (4, 1), (5, 2), (6, 2), (7, 3), (8, 3);

INSERT INTO Jugadores (Id, EquipoId, Nombre, Posicion, CreadoEn)
SELECT NEWID(), e.Id, e.Nombre + ' - Jugador ' + CAST(p.Num AS NVARCHAR(2)), p.Posicion, SYSUTCDATETIME()
FROM @EquiposNuevos e
CROSS JOIN @Posiciones p;

------------------------------------------------------------------------------
-- 3. Partidos: 80 emparejamientos al azar entre TODOS los equipos (viejos +
--    nuevos), con fecha entre -90 y +59 dias desde hoy.
------------------------------------------------------------------------------
CREATE TABLE #Fixtures (
    RowNum INT IDENTITY(1,1) PRIMARY KEY,
    PartidoId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Local UNIQUEIDENTIFIER NOT NULL,
    Visitante UNIQUEIDENTIFIER NOT NULL,
    FechaHora DATETIME2 NULL,
    Estado INT NULL,
    GolesLocal INT NULL,
    GolesVisitante INT NULL,
    FechaResultadoRegistrado DATETIME2 NULL
);

INSERT INTO #Fixtures (Local, Visitante)
SELECT TOP 80 a.Id, b.Id
FROM Equipos a
CROSS JOIN Equipos b
WHERE a.Id <> b.Id
ORDER BY NEWID();

-- Fecha al azar (ABS(CHECKSUM(NEWID())) % N es el truco estandar para que
-- RAND() varie por fila en SQL Server, ya que RAND() sin semilla se evalua
-- una sola vez por consulta).
UPDATE #Fixtures
SET FechaHora = DATEADD(DAY, (ABS(CHECKSUM(NEWID())) % 150) - 90, SYSUTCDATETIME()),
    GolesLocal = ABS(CHECKSUM(NEWID())) % 6,
    GolesVisitante = ABS(CHECKSUM(NEWID())) % 6;

-- Estado: si la fecha ya paso, 85% de probabilidad de que ya se jugo
-- (el resto queda Programado pese a ser pasado, como partido pospuesto);
-- si la fecha es futura, siempre Programado.
UPDATE #Fixtures
SET Estado = CASE
    WHEN FechaHora < SYSUTCDATETIME() AND ABS(CHECKSUM(NEWID())) % 100 < 85 THEN 1
    ELSE 0
END;

-- Un partido Programado no tiene resultado todavia.
UPDATE #Fixtures
SET GolesLocal = NULL, GolesVisitante = NULL, FechaResultadoRegistrado = NULL
WHERE Estado = 0;

UPDATE #Fixtures
SET FechaResultadoRegistrado = DATEADD(HOUR, 2, FechaHora)
WHERE Estado = 1;

INSERT INTO Partidos (Id, EquipoLocalId, EquipoVisitanteId, FechaHora, Estado, GolesLocal, GolesVisitante, FechaResultadoRegistrado, CreadoEn)
SELECT PartidoId, Local, Visitante, FechaHora, Estado, GolesLocal, GolesVisitante, FechaResultadoRegistrado, SYSUTCDATETIME()
FROM #Fixtures;

------------------------------------------------------------------------------
-- 4. Goleadores: para cada partido Jugado con marcador > 0, se le carga el
--    marcador completo a UN jugador al azar de ese equipo (simplificacion
--    deliberada: alcanza para tener variedad de CantidadGoles al ordenar en
--    "GET /goleadores", sin necesitar repartir un mismo marcador entre
--    varios jugadores).
------------------------------------------------------------------------------
INSERT INTO GolesPartido (Id, PartidoId, JugadorId, CantidadGoles, CreadoEn)
SELECT NEWID(), f.PartidoId, j.JugadorId, f.GolesLocal, SYSUTCDATETIME()
FROM #Fixtures f
CROSS APPLY (
    SELECT TOP 1 Id AS JugadorId FROM Jugadores WHERE EquipoId = f.Local ORDER BY NEWID()
) j
WHERE f.Estado = 1 AND f.GolesLocal > 0;

INSERT INTO GolesPartido (Id, PartidoId, JugadorId, CantidadGoles, CreadoEn)
SELECT NEWID(), f.PartidoId, j.JugadorId, f.GolesVisitante, SYSUTCDATETIME()
FROM #Fixtures f
CROSS APPLY (
    SELECT TOP 1 Id AS JugadorId FROM Jugadores WHERE EquipoId = f.Visitante ORDER BY NEWID()
) j
WHERE f.Estado = 1 AND f.GolesVisitante > 0;

DROP TABLE #Fixtures;

------------------------------------------------------------------------------
-- Resumen
------------------------------------------------------------------------------
SELECT
    (SELECT COUNT(*) FROM Equipos)      AS TotalEquipos,
    (SELECT COUNT(*) FROM Jugadores)    AS TotalJugadores,
    (SELECT COUNT(*) FROM Partidos)     AS TotalPartidos,
    (SELECT COUNT(*) FROM Partidos WHERE Estado = 1) AS PartidosJugados,
    (SELECT COUNT(*) FROM GolesPartido) AS TotalRegistrosGoles;
