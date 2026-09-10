"use client";

import { useEffect, useState, type FormEvent } from "react";
import { useParams } from "next/navigation";
import Link from "next/link";
import { obtenerEquipo, actualizarEquipo } from "@/services/equiposService";
import {
  listarJugadoresDeEquipo,
  registrarJugador,
  type OrdenJugadores,
} from "@/services/jugadoresService";
import { ErrorApiCliente } from "@/services/apiClient";
import type { Equipo } from "@/types/equipo";
import type { Jugador, PosicionJugador } from "@/types/jugador";
import { POSICIONES_JUGADOR } from "@/types/jugador";
import { usePaginacion } from "@/hooks/usePaginacion";
import { useOrdenColumna } from "@/hooks/useOrdenColumna";
import MensajeError from "@/components/MensajeError";
import Paginador from "@/components/Paginador";
import Tarjeta from "@/components/ui/Tarjeta";
import Campo from "@/components/ui/Campo";
import Input from "@/components/ui/Input";
import Select from "@/components/ui/Select";
import Boton from "@/components/ui/Boton";
import Badge from "@/components/ui/Badge";
import EncabezadoOrdenable from "@/components/ui/EncabezadoOrdenable";

type FiltroPosicion = PosicionJugador | "";

export default function DetalleEquipoPage() {
  const { id } = useParams<{ id: string }>();

  const [equipo, setEquipo] = useState<Equipo | null>(null);
  const [jugadores, setJugadores] = useState<Jugador[]>([]);
  const [cargando, setCargando] = useState(true);
  const [cargandoJugadores, setCargandoJugadores] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [nombreEquipo, setNombreEquipo] = useState("");
  const [ciudadEquipo, setCiudadEquipo] = useState("");
  const [guardandoEquipo, setGuardandoEquipo] = useState(false);

  const [nombreJugador, setNombreJugador] = useState("");
  const [posicion, setPosicion] = useState<PosicionJugador>("Delantero");

  const [filtroNombre, setFiltroNombre] = useState("");
  const [filtroPosicion, setFiltroPosicion] = useState<FiltroPosicion>("");
  const [filtrosAplicados, setFiltrosAplicados] = useState({ nombre: "", posicion: "" as FiltroPosicion });

  const orden = useOrdenColumna<OrdenJugadores>("nombre");
  const pag = usePaginacion();

  async function cargarEquipo() {
    setCargando(true);
    try {
      const equipoCargado = await obtenerEquipo(id);
      setEquipo(equipoCargado);
      setNombreEquipo(equipoCargado.nombre);
      setCiudadEquipo(equipoCargado.ciudadOrigen);
      setError(null);
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo cargar el equipo.");
    } finally {
      setCargando(false);
    }
  }

  async function cargarJugadores() {
    setCargandoJugadores(true);
    try {
      const resultado = await listarJugadoresDeEquipo(id, {
        nombre: filtrosAplicados.nombre || undefined,
        posicion: filtrosAplicados.posicion || undefined,
        numeroPagina: pag.numeroPagina,
        tamanoPagina: pag.tamanoPagina,
        ordenarPor: orden.ordenarPor,
        direccionOrden: orden.direccionOrden,
      });
      setJugadores(resultado.elementos);
      pag.actualizarDesdeResultado(resultado);
      setError(null);
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo cargar el roster del equipo.");
    } finally {
      setCargandoJugadores(false);
    }
  }

  useEffect(() => {
    cargarEquipo();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  useEffect(() => {
    cargarJugadores();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id, pag.numeroPagina, pag.tamanoPagina, orden.ordenarPor, orden.direccionOrden, filtrosAplicados]);

  function manejarBuscar(evento: FormEvent) {
    evento.preventDefault();
    pag.reiniciar();
    setFiltrosAplicados({ nombre: filtroNombre, posicion: filtroPosicion });
  }

  function limpiarFiltros() {
    setFiltroNombre("");
    setFiltroPosicion("");
    pag.reiniciar();
    setFiltrosAplicados({ nombre: "", posicion: "" });
  }

  async function manejarGuardarEquipo(evento: FormEvent) {
    evento.preventDefault();
    setError(null);
    setGuardandoEquipo(true);
    try {
      await actualizarEquipo(id, { nombre: nombreEquipo, ciudadOrigen: ciudadEquipo });
      await cargarEquipo();
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo actualizar el equipo.");
    } finally {
      setGuardandoEquipo(false);
    }
  }

  async function manejarRegistrarJugador(evento: FormEvent) {
    evento.preventDefault();
    setError(null);
    try {
      await registrarJugador(id, { nombre: nombreJugador, posicion });
      setNombreJugador("");
      await cargarJugadores();
    } catch (err) {
      setError(err instanceof ErrorApiCliente ? err.message : "No se pudo registrar el jugador.");
    }
  }

  if (cargando) return <p className="text-sm text-slate-500">Cargando...</p>;
  if (!equipo) return <MensajeError mensaje={error ?? "Equipo no encontrado."} />;

  return (
    <div className="flex flex-col gap-6">
      <div>
        <Link href="/equipos" className="text-sm font-medium text-emerald-700 hover:underline">
          ← Volver a equipos
        </Link>
        <h1 className="mt-2 text-2xl font-bold text-slate-900">{equipo.nombre}</h1>
        <p className="mt-1 text-sm text-slate-500">Ciudad de origen: {equipo.ciudadOrigen}</p>
      </div>

      <MensajeError mensaje={error} />

      <Tarjeta titulo="Editar equipo">
        <form onSubmit={manejarGuardarEquipo} className="flex flex-wrap items-end gap-3">
          <Campo etiqueta="Nombre" className="min-w-48 flex-1">
            <Input value={nombreEquipo} onChange={(e) => setNombreEquipo(e.target.value)} required />
          </Campo>
          <Campo etiqueta="Ciudad de origen" className="min-w-48 flex-1">
            <Input value={ciudadEquipo} onChange={(e) => setCiudadEquipo(e.target.value)} required />
          </Campo>
          <Boton type="submit" variante="primario" disabled={guardandoEquipo}>
            {guardandoEquipo ? "Guardando..." : "Guardar cambios"}
          </Boton>
        </form>
      </Tarjeta>

      <Tarjeta titulo="Integrantes">
        <form
          onSubmit={manejarBuscar}
          className="flex flex-wrap items-end gap-3 border-b border-slate-100 pb-4"
        >
          <Campo etiqueta="Buscar por nombre" className="min-w-44 flex-1">
            <Input value={filtroNombre} onChange={(e) => setFiltroNombre(e.target.value)} />
          </Campo>
          <Campo etiqueta="Posición" className="min-w-40">
            <Select value={filtroPosicion} onChange={(e) => setFiltroPosicion(e.target.value as FiltroPosicion)}>
              <option value="">Todas</option>
              {POSICIONES_JUGADOR.map((pos) => (
                <option key={pos} value={pos}>
                  {pos}
                </option>
              ))}
            </Select>
          </Campo>
          <Boton type="submit" variante="primario">
            Buscar
          </Boton>
          <Boton type="button" variante="fantasma" onClick={limpiarFiltros}>
            Limpiar
          </Boton>
        </form>

        {cargandoJugadores ? (
          <p className="mt-4 text-sm text-slate-500">Cargando...</p>
        ) : (
          <>
            <p className="mt-4 mb-2 text-sm text-slate-500">{pag.totalRegistros} jugador(es) encontrado(s)</p>
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
                      columna="posicion"
                      ordenActual={orden.ordenarPor}
                      direccionActual={orden.direccionOrden}
                      onOrdenar={(c) => {
                        orden.ordenarPorColumna(c);
                        pag.reiniciar();
                      }}
                    >
                      Posición
                    </EncabezadoOrdenable>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {jugadores.map((jugador) => (
                    <tr key={jugador.id} className="hover:bg-slate-50">
                      <td className="px-3 py-2.5 font-medium text-slate-800">{jugador.nombre}</td>
                      <td className="px-3 py-2.5">
                        <Badge>{jugador.posicion}</Badge>
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

      <Tarjeta titulo="Registrar jugador">
        <form onSubmit={manejarRegistrarJugador} className="flex flex-wrap items-end gap-3">
          <Campo etiqueta="Nombre" className="min-w-48 flex-1">
            <Input value={nombreJugador} onChange={(e) => setNombreJugador(e.target.value)} required />
          </Campo>
          <Campo etiqueta="Posición" className="min-w-40">
            <Select value={posicion} onChange={(e) => setPosicion(e.target.value as PosicionJugador)}>
              {POSICIONES_JUGADOR.map((pos) => (
                <option key={pos} value={pos}>
                  {pos}
                </option>
              ))}
            </Select>
          </Campo>
          <Boton type="submit" variante="primario">
            Agregar jugador
          </Boton>
        </form>
      </Tarjeta>
    </div>
  );
}
