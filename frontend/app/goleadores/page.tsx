"use client";

import { useEffect, useState } from "react";
import { obtenerGoleadores, type OrdenGoleadores } from "@/services/goleadoresService";
import { listarEquipos } from "@/services/equiposService";
import { ErrorApiCliente } from "@/services/apiClient";
import type { Goleador } from "@/types/goleador";
import type { Equipo } from "@/types/equipo";
import { usePaginacion } from "@/hooks/usePaginacion";
import { useOrdenColumna } from "@/hooks/useOrdenColumna";
import MensajeError from "@/components/MensajeError";
import Paginador from "@/components/Paginador";
import Tarjeta from "@/components/ui/Tarjeta";
import Campo from "@/components/ui/Campo";
import Select from "@/components/ui/Select";
import EncabezadoOrdenable from "@/components/ui/EncabezadoOrdenable";

const DIRECCION_POR_DEFECTO: Partial<Record<OrdenGoleadores, "asc" | "desc">> = {
  cantidadGoles: "desc",
  nombreJugador: "asc",
};

export default function GoleadoresPage() {
  const [goleadores, setGoleadores] = useState<Goleador[]>([]);
  const [equipos, setEquipos] = useState<Equipo[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [filtroEquipoId, setFiltroEquipoId] = useState("");
  const orden = useOrdenColumna<OrdenGoleadores>("cantidadGoles", "desc", DIRECCION_POR_DEFECTO);
  const pag = usePaginacion();

  useEffect(() => {
    listarEquipos({ tamanoPagina: 100 })
      .then((resultado) => setEquipos(resultado.elementos))
      .catch(() => {});
  }, []);

  useEffect(() => {
    setCargando(true);
    obtenerGoleadores({
      equipoId: filtroEquipoId || undefined,
      numeroPagina: pag.numeroPagina,
      tamanoPagina: pag.tamanoPagina,
      ordenarPor: orden.ordenarPor,
      direccionOrden: orden.direccionOrden,
    })
      .then((resultado) => {
        setGoleadores(resultado.elementos);
        pag.actualizarDesdeResultado(resultado);
        setError(null);
      })
      .catch((err) =>
        setError(err instanceof ErrorApiCliente ? err.message : "No se pudo cargar la lista de goleadores.")
      )
      .finally(() => setCargando(false));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filtroEquipoId, pag.numeroPagina, pag.tamanoPagina, orden.ordenarPor, orden.direccionOrden]);

  const esRankingPorGoles = orden.ordenarPor === "cantidadGoles" && orden.direccionOrden === "desc";

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-900">Goleadores</h1>
        <p className="mt-1 text-sm text-slate-500">Ranking de goleadores del torneo.</p>
      </div>

      <MensajeError mensaje={error} />

      <Tarjeta>
        <Campo etiqueta="Filtrar por equipo" className="max-w-xs">
          <Select
            value={filtroEquipoId}
            onChange={(e) => {
              setFiltroEquipoId(e.target.value);
              pag.reiniciar();
            }}
          >
            <option value="">Todos los equipos</option>
            {equipos.map((equipo) => (
              <option key={equipo.id} value={equipo.id}>
                {equipo.nombre}
              </option>
            ))}
          </Select>
        </Campo>

        {cargando ? (
          <p className="mt-6 text-sm text-slate-500">Cargando...</p>
        ) : (
          <>
            <p className="mt-5 mb-2 text-sm text-slate-500">{pag.totalRegistros} goleador(es) encontrado(s)</p>
            <div className="overflow-x-auto rounded-lg border border-slate-200">
              <table className="min-w-full divide-y divide-slate-100 text-sm">
                <thead className="bg-slate-50">
                  <tr>
                    <EncabezadoOrdenable
                      columna="nombreJugador"
                      ordenActual={orden.ordenarPor}
                      direccionActual={orden.direccionOrden}
                      onOrdenar={(c) => {
                        orden.ordenarPorColumna(c);
                        pag.reiniciar();
                      }}
                    >
                      Jugador
                    </EncabezadoOrdenable>
                    <th className="px-3 py-2.5 text-left text-xs font-semibold uppercase tracking-wide text-slate-500">
                      Equipo
                    </th>
                    <EncabezadoOrdenable
                      columna="cantidadGoles"
                      ordenActual={orden.ordenarPor}
                      direccionActual={orden.direccionOrden}
                      onOrdenar={(c) => {
                        orden.ordenarPorColumna(c);
                        pag.reiniciar();
                      }}
                    >
                      Goles
                    </EncabezadoOrdenable>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {goleadores.map((goleador, indice) => (
                    <tr key={goleador.jugadorId} className="hover:bg-slate-50">
                      <td className="px-3 py-2.5 font-medium text-slate-800">
                        {esRankingPorGoles && pag.numeroPagina === 1 && indice < 3 && (
                          <span className="mr-1.5">🏆</span>
                        )}
                        {goleador.nombreJugador}
                      </td>
                      <td className="px-3 py-2.5 text-slate-600">{goleador.nombreEquipo}</td>
                      <td className="px-3 py-2.5 font-semibold text-emerald-700">{goleador.cantidadGoles}</td>
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
    </div>
  );
}
