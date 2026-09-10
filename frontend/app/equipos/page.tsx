"use client";

import { useEffect, useState, type FormEvent } from "react";
import Link from "next/link";
import { listarEquipos, crearEquipo, eliminarEquipo, type OrdenEquipos } from "@/services/equiposService";
import { ErrorApiCliente } from "@/services/apiClient";
import type { Equipo } from "@/types/equipo";
import { usePaginacion } from "@/hooks/usePaginacion";
import { useOrdenColumna } from "@/hooks/useOrdenColumna";
import MensajeError from "@/components/MensajeError";
import Paginador from "@/components/Paginador";
import Tarjeta from "@/components/ui/Tarjeta";
import Campo from "@/components/ui/Campo";
import Input from "@/components/ui/Input";
import Boton from "@/components/ui/Boton";
import EncabezadoOrdenable from "@/components/ui/EncabezadoOrdenable";

export default function EquiposPage() {
  const [equipos, setEquipos] = useState<Equipo[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [nombre, setNombre] = useState("");
  const [ciudadOrigen, setCiudadOrigen] = useState("");

  const [filtroNombre, setFiltroNombre] = useState("");
  const [filtroCiudad, setFiltroCiudad] = useState("");
  const [filtrosAplicados, setFiltrosAplicados] = useState({ nombre: "", ciudadOrigen: "" });

  const orden = useOrdenColumna<OrdenEquipos>("nombre");
  const pag = usePaginacion();

  async function cargarEquipos() {
    setCargando(true);
    try {
      const resultado = await listarEquipos({
        nombre: filtrosAplicados.nombre || undefined,
        ciudadOrigen: filtrosAplicados.ciudadOrigen || undefined,
        numeroPagina: pag.numeroPagina,
        tamanoPagina: pag.tamanoPagina,
        ordenarPor: orden.ordenarPor,
        direccionOrden: orden.direccionOrden,
      });
      setEquipos(resultado.elementos);
      pag.actualizarDesdeResultado(resultado);
      setError(null);
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo cargar la lista de equipos.");
    } finally {
      setCargando(false);
    }
  }

  useEffect(() => {
    cargarEquipos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pag.numeroPagina, pag.tamanoPagina, orden.ordenarPor, orden.direccionOrden, filtrosAplicados]);

  function manejarBuscar(evento: FormEvent) {
    evento.preventDefault();
    pag.reiniciar();
    setFiltrosAplicados({ nombre: filtroNombre, ciudadOrigen: filtroCiudad });
  }

  function limpiarFiltros() {
    setFiltroNombre("");
    setFiltroCiudad("");
    pag.reiniciar();
    setFiltrosAplicados({ nombre: "", ciudadOrigen: "" });
  }

  async function manejarCrear(evento: FormEvent) {
    evento.preventDefault();
    setError(null);
    try {
      await crearEquipo({ nombre, ciudadOrigen });
      setNombre("");
      setCiudadOrigen("");
      await cargarEquipos();
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo crear el equipo.");
    }
  }

  async function manejarEliminar(id: string) {
    setError(null);
    try {
      await eliminarEquipo(id);
      await cargarEquipos();
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo eliminar el equipo.");
    }
  }

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-900">Equipos</h1>
        <p className="mt-1 text-sm text-slate-500">Administrá los equipos del torneo.</p>
      </div>

      <Tarjeta titulo="Crear equipo">
        <form onSubmit={manejarCrear} className="flex flex-wrap items-end gap-3">
          <Campo etiqueta="Nombre" className="min-w-48 flex-1">
            <Input value={nombre} onChange={(e) => setNombre(e.target.value)} required />
          </Campo>
          <Campo etiqueta="Ciudad de origen" className="min-w-48 flex-1">
            <Input value={ciudadOrigen} onChange={(e) => setCiudadOrigen(e.target.value)} required />
          </Campo>
          <Boton type="submit" variante="primario">
            Crear equipo
          </Boton>
        </form>
      </Tarjeta>

      <MensajeError mensaje={error} />

      <Tarjeta>
        <form
          onSubmit={manejarBuscar}
          className="flex flex-wrap items-end gap-3"
        >
          <Campo etiqueta="Buscar por nombre" className="min-w-44 flex-1">
            <Input value={filtroNombre} onChange={(e) => setFiltroNombre(e.target.value)} />
          </Campo>
          <Campo etiqueta="Buscar por ciudad" className="min-w-44 flex-1">
            <Input value={filtroCiudad} onChange={(e) => setFiltroCiudad(e.target.value)} />
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
            <p className="mt-5 mb-2 text-sm text-slate-500">{pag.totalRegistros} equipo(s) encontrado(s)</p>
            <div className="overflow-x-auto rounded-lg border border-slate-200">
              <table className="min-w-full divide-y divide-slate-100 text-sm">
                <thead className="bg-slate-50">
                  <tr>
                    <EncabezadoOrdenable
                      columna="nombre"
                      ordenActual={orden.ordenarPor}
                      direccionActual={orden.direccionOrden}
                      onOrdenar={(c) => {
                        orden.ordenarPorColumna(c);
                        pag.reiniciar();
                      }}
                    >
                      Nombre
                    </EncabezadoOrdenable>
                    <EncabezadoOrdenable
                      columna="ciudadOrigen"
                      ordenActual={orden.ordenarPor}
                      direccionActual={orden.direccionOrden}
                      onOrdenar={(c) => {
                        orden.ordenarPorColumna(c);
                        pag.reiniciar();
                      }}
                    >
                      Ciudad de origen
                    </EncabezadoOrdenable>
                    <th className="px-3 py-2.5"></th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {equipos.map((equipo) => (
                    <tr key={equipo.id} className="hover:bg-slate-50">
                      <td className="px-3 py-2.5">
                        <Link href={`/equipos/${equipo.id}`} className="font-medium text-emerald-700 hover:underline">
                          {equipo.nombre}
                        </Link>
                      </td>
                      <td className="px-3 py-2.5 text-slate-600">{equipo.ciudadOrigen}</td>
                      <td className="px-3 py-2.5 text-right">
                        <Boton variante="peligro" onClick={() => manejarEliminar(equipo.id)}>
                          Eliminar
                        </Boton>
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
    </div>
  );
}
