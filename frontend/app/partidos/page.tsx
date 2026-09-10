"use client";

import { useEffect, useState, type FormEvent } from "react";
import { listarPartidos, crearPartido, registrarResultado, type OrdenPartidos } from "@/services/partidosService";
import { listarEquipos } from "@/services/equiposService";
import { listarJugadoresDeEquipo } from "@/services/jugadoresService";
import { ErrorApiCliente } from "@/services/apiClient";
import type { EstadoPartido, Partido } from "@/types/partido";
import type { Equipo } from "@/types/equipo";
import type { Jugador } from "@/types/jugador";
import { usePaginacion } from "@/hooks/usePaginacion";
import { useOrdenColumna } from "@/hooks/useOrdenColumna";
import MensajeError from "@/components/MensajeError";
import FormularioResultado from "@/components/FormularioResultado";
import Paginador from "@/components/Paginador";
import Tarjeta from "@/components/ui/Tarjeta";
import Modal from "@/components/ui/Modal";
import Campo from "@/components/ui/Campo";
import Input from "@/components/ui/Input";
import Select from "@/components/ui/Select";
import Boton from "@/components/ui/Boton";
import Badge from "@/components/ui/Badge";
import EncabezadoOrdenable from "@/components/ui/EncabezadoOrdenable";

type FiltroEstado = EstadoPartido | "";

export default function PartidosPage() {
  const [partidos, setPartidos] = useState<Partido[]>([]);
  const [equipos, setEquipos] = useState<Equipo[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [equipoLocalId, setEquipoLocalId] = useState("");
  const [equipoVisitanteId, setEquipoVisitanteId] = useState("");
  const [fechaHora, setFechaHora] = useState("");

  const [partidoSeleccionado, setPartidoSeleccionado] = useState<Partido | null>(null);
  const [jugadoresLocal, setJugadoresLocal] = useState<Jugador[]>([]);
  const [jugadoresVisitante, setJugadoresVisitante] = useState<Jugador[]>([]);

  const [filtroFechaDesde, setFiltroFechaDesde] = useState("");
  const [filtroFechaHasta, setFiltroFechaHasta] = useState("");
  const [filtroEquipoId, setFiltroEquipoId] = useState("");
  const [filtroEstado, setFiltroEstado] = useState<FiltroEstado>("");
  const [filtrosAplicados, setFiltrosAplicados] = useState({
    fechaDesde: "",
    fechaHasta: "",
    equipoId: "",
    estado: "" as FiltroEstado,
  });

  const orden = useOrdenColumna<OrdenPartidos>("fechaHora");
  const pag = usePaginacion();

  async function cargarDatos() {
    setCargando(true);
    try {
      const [partidosCargados, equiposCargados] = await Promise.all([
        listarPartidos({
          fechaDesde: filtrosAplicados.fechaDesde || undefined,
          fechaHasta: filtrosAplicados.fechaHasta || undefined,
          equipoId: filtrosAplicados.equipoId || undefined,
          estado: filtrosAplicados.estado || undefined,
          numeroPagina: pag.numeroPagina,
          tamanoPagina: pag.tamanoPagina,
          ordenarPor: orden.ordenarPor,
          direccionOrden: orden.direccionOrden,
        }),
        listarEquipos({ tamanoPagina: 100 }),
      ]);
      setPartidos(partidosCargados.elementos);
      pag.actualizarDesdeResultado(partidosCargados);
      setEquipos(equiposCargados.elementos);
      setError(null);
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo cargar la lista de partidos.");
    } finally {
      setCargando(false);
    }
  }

  useEffect(() => {
    cargarDatos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pag.numeroPagina, pag.tamanoPagina, orden.ordenarPor, orden.direccionOrden, filtrosAplicados]);

  function manejarBuscar(evento: FormEvent) {
    evento.preventDefault();
    pag.reiniciar();
    setFiltrosAplicados({
      fechaDesde: filtroFechaDesde,
      fechaHasta: filtroFechaHasta,
      equipoId: filtroEquipoId,
      estado: filtroEstado,
    });
  }

  function limpiarFiltros() {
    setFiltroFechaDesde("");
    setFiltroFechaHasta("");
    setFiltroEquipoId("");
    setFiltroEstado("");
    pag.reiniciar();
    setFiltrosAplicados({ fechaDesde: "", fechaHasta: "", equipoId: "", estado: "" });
  }

  async function manejarCrear(evento: FormEvent) {
    evento.preventDefault();
    setError(null);
    try {
      await crearPartido({ equipoLocalId, equipoVisitanteId, fechaHora: new Date(fechaHora).toISOString() });
      setEquipoLocalId("");
      setEquipoVisitanteId("");
      setFechaHora("");
      await cargarDatos();
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo crear el partido.");
    }
  }

  async function manejarSeleccionarPartido(partido: Partido) {
    setError(null);
    setPartidoSeleccionado(partido);
    try {
      const [local, visitante] = await Promise.all([
        listarJugadoresDeEquipo(partido.equipoLocalId, { tamanoPagina: 100 }),
        listarJugadoresDeEquipo(partido.equipoVisitanteId, { tamanoPagina: 100 }),
      ]);
      setJugadoresLocal(local.elementos);
      setJugadoresVisitante(visitante.elementos);
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudieron cargar los jugadores del partido.");
    }
  }

  async function manejarRegistrarResultado(datos: {
    golesLocal: number;
    golesVisitante: number;
    goleadores: { jugadorId: string; cantidadGoles: number }[];
  }) {
    if (!partidoSeleccionado) return;
    setError(null);
    try {
      await registrarResultado(partidoSeleccionado.id, datos);
      setPartidoSeleccionado(null);
      await cargarDatos();
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo registrar el resultado.");
    }
  }

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-900">Partidos</h1>
        <p className="mt-1 text-sm text-slate-500">Programá partidos y registrá resultados.</p>
      </div>

      <Tarjeta titulo="Crear partido">
        <form onSubmit={manejarCrear} className="flex flex-wrap items-end gap-3">
          <Campo etiqueta="Equipo local" className="min-w-44 flex-1">
            <Select value={equipoLocalId} onChange={(e) => setEquipoLocalId(e.target.value)} required>
              <option value="">Seleccionar...</option>
              {equipos.map((equipo) => (
                <option key={equipo.id} value={equipo.id}>
                  {equipo.nombre}
                </option>
              ))}
            </Select>
          </Campo>
          <Campo etiqueta="Equipo visitante" className="min-w-44 flex-1">
            <Select value={equipoVisitanteId} onChange={(e) => setEquipoVisitanteId(e.target.value)} required>
              <option value="">Seleccionar...</option>
              {equipos.map((equipo) => (
                <option key={equipo.id} value={equipo.id}>
                  {equipo.nombre}
                </option>
              ))}
            </Select>
          </Campo>
          <Campo etiqueta="Fecha y hora" className="min-w-48">
            <Input type="datetime-local" value={fechaHora} onChange={(e) => setFechaHora(e.target.value)} required />
          </Campo>
          <Boton type="submit" variante="primario">
            Crear partido
          </Boton>
        </form>
      </Tarjeta>

      <MensajeError mensaje={error} />

      <Tarjeta>
        <form
          onSubmit={manejarBuscar}
          className="flex flex-wrap items-end gap-3"
        >
          <Campo etiqueta="Desde" className="min-w-36">
            <Input type="date" value={filtroFechaDesde} onChange={(e) => setFiltroFechaDesde(e.target.value)} />
          </Campo>
          <Campo etiqueta="Hasta" className="min-w-36">
            <Input type="date" value={filtroFechaHasta} onChange={(e) => setFiltroFechaHasta(e.target.value)} />
          </Campo>
          <Campo etiqueta="Equipo" className="min-w-44 flex-1">
            <Select value={filtroEquipoId} onChange={(e) => setFiltroEquipoId(e.target.value)}>
              <option value="">Todos los equipos</option>
              {equipos.map((equipo) => (
                <option key={equipo.id} value={equipo.id}>
                  {equipo.nombre}
                </option>
              ))}
            </Select>
          </Campo>
          <Campo etiqueta="Estado" className="min-w-40">
            <Select value={filtroEstado} onChange={(e) => setFiltroEstado(e.target.value as FiltroEstado)}>
              <option value="">Todos</option>
              <option value="Programado">Programado</option>
              <option value="Jugado">Jugado</option>
            </Select>
          </Campo>
          <Boton type="submit" variante="primario">
            Buscar
          </Boton>
          <Boton type="button" variante="fantasma" onClick={limpiarFiltros}>
            Limpiar
          </Boton>
        </form>

        {cargando ? (
          <p className="mt-6 text-sm text-slate-500">Cargando...</p>
        ) : (
          <>
            <p className="mt-5 mb-2 text-sm text-slate-500">{pag.totalRegistros} partido(s) encontrado(s)</p>
            <div className="overflow-x-auto rounded-lg border border-slate-200">
              <table className="min-w-full divide-y divide-slate-100 text-sm">
                <thead className="bg-slate-50">
                  <tr>
                    <th className="px-3 py-2.5 text-left text-xs font-semibold uppercase tracking-wide text-slate-500">
                      Local
                    </th>
                    <th className="px-3 py-2.5 text-left text-xs font-semibold uppercase tracking-wide text-slate-500">
                      Visitante
                    </th>
                    <EncabezadoOrdenable
                      columna="fechaHora"
                      ordenActual={orden.ordenarPor}
                      direccionActual={orden.direccionOrden}
                      onOrdenar={(c) => {
                        orden.ordenarPorColumna(c);
                        pag.reiniciar();
                      }}
                    >
                      Fecha
                    </EncabezadoOrdenable>
                    <EncabezadoOrdenable
                      columna="estado"
                      ordenActual={orden.ordenarPor}
                      direccionActual={orden.direccionOrden}
                      onOrdenar={(c) => {
                        orden.ordenarPorColumna(c);
                        pag.reiniciar();
                      }}
                    >
                      Estado
                    </EncabezadoOrdenable>
                    <th className="px-3 py-2.5 text-left text-xs font-semibold uppercase tracking-wide text-slate-500">
                      Resultado
                    </th>
                    <th className="px-3 py-2.5"></th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {partidos.map((partido) => (
                    <tr key={partido.id} className="hover:bg-slate-50">
                      <td className="px-3 py-2.5 font-medium text-slate-800">{partido.nombreEquipoLocal}</td>
                      <td className="px-3 py-2.5 text-slate-600">{partido.nombreEquipoVisitante}</td>
                      <td className="px-3 py-2.5 text-slate-600">{new Date(partido.fechaHora).toLocaleString()}</td>
                      <td className="px-3 py-2.5">
                        <Badge tono={partido.estado === "Jugado" ? "exito" : "neutro"}>{partido.estado}</Badge>
                      </td>
                      <td className="px-3 py-2.5 text-slate-600">
                        {partido.estado === "Jugado" ? `${partido.golesLocal} - ${partido.golesVisitante}` : "—"}
                      </td>
                      <td className="px-3 py-2.5 text-right">
                        {partido.estado === "Programado" && (
                          <Boton variante="secundario" onClick={() => manejarSeleccionarPartido(partido)}>
                            Registrar resultado
                          </Boton>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <Paginador
              numeroPagina={pag.numeroPagina}
              totalPaginas={pag.totalPaginas}
              onCambiarPagina={pag.setNumeroPagina}
              tamanoPagina={pag.tamanoPagina}
              onCambiarTamanoPagina={pag.cambiarTamanoPagina}
            />
          </>
        )}
      </Tarjeta>

      {partidoSeleccionado && (
        <Modal
          titulo={`Resultado: ${partidoSeleccionado.nombreEquipoLocal} vs ${partidoSeleccionado.nombreEquipoVisitante}`}
          onCerrar={() => setPartidoSeleccionado(null)}
        >
          <FormularioResultado
            jugadoresLocal={jugadoresLocal}
            jugadoresVisitante={jugadoresVisitante}
            onRegistrar={manejarRegistrarResultado}
          />
        </Modal>
      )}
    </div>
  );
}
