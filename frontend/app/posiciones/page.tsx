"use client";

import { useEffect, useState } from "react";
import { obtenerPosiciones } from "@/services/posicionesService";
import { ErrorApiCliente } from "@/services/apiClient";
import type { Posicion } from "@/types/posicion";
import { usePaginacion } from "@/hooks/usePaginacion";
import MensajeError from "@/components/MensajeError";
import Paginador from "@/components/Paginador";
import Tarjeta from "@/components/ui/Tarjeta";

const COLUMNAS = ["#", "Equipo", "PJ", "G", "E", "P", "GF", "GC", "DG", "Pts"];

export default function PosicionesPage() {
  const [tabla, setTabla] = useState<Posicion[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const pag = usePaginacion();

  useEffect(() => {
    setCargando(true);
    obtenerPosiciones(pag.numeroPagina, pag.tamanoPagina)
      .then((resultado) => {
        setTabla(resultado.elementos);
        pag.actualizarDesdeResultado(resultado);
        setError(null);
      })
      .catch((err) =>
        setError(err instanceof ErrorApiCliente ? err.message : "No se pudo cargar la tabla de posiciones.")
      )
      .finally(() => setCargando(false));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pag.numeroPagina, pag.tamanoPagina]);

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-900">Tabla de posiciones</h1>
        <p className="mt-1 text-sm text-slate-500">
          Orden fijo por reglamento: puntos, luego diferencia de gol, luego goles a favor.
        </p>
      </div>

      <MensajeError mensaje={error} />

      <Tarjeta>
        {cargando ? (
          <p className="text-sm text-slate-500">Cargando...</p>
        ) : (
          <>
            <div className="overflow-x-auto rounded-lg border border-slate-200">
              <table className="min-w-full divide-y divide-slate-100 text-sm">
                <thead className="bg-slate-50">
                  <tr>
                    {COLUMNAS.map((columna) => (
                      <th
                        key={columna}
                        className={`px-3 py-2.5 text-xs font-semibold uppercase tracking-wide text-slate-500 ${
                          columna === "Equipo" ? "text-left" : "text-center"
                        }`}
                      >
                        {columna}
                      </th>
                    ))}
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {tabla.map((fila) => (
                    <tr key={fila.equipoId} className="hover:bg-slate-50">
                      <td className="px-3 py-2.5 text-center font-semibold text-slate-500">{fila.posicion}</td>
                      <td className="px-3 py-2.5 font-medium text-slate-800">{fila.nombreEquipo}</td>
                      <td className="px-3 py-2.5 text-center text-slate-600">{fila.partidosJugados}</td>
                      <td className="px-3 py-2.5 text-center text-slate-600">{fila.victorias}</td>
                      <td className="px-3 py-2.5 text-center text-slate-600">{fila.empates}</td>
                      <td className="px-3 py-2.5 text-center text-slate-600">{fila.derrotas}</td>
                      <td className="px-3 py-2.5 text-center text-slate-600">{fila.golesFavor}</td>
                      <td className="px-3 py-2.5 text-center text-slate-600">{fila.golesContra}</td>
                      <td className="px-3 py-2.5 text-center text-slate-600">{fila.diferenciaGol}</td>
                      <td className="px-3 py-2.5 text-center font-bold text-emerald-700">{fila.puntos}</td>
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
