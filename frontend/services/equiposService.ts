import { obtenerJson, enviarJson } from "./apiClient";
import type { Equipo } from "@/types/equipo";
import type { ResultadoPaginado } from "@/types/paginado";

export type OrdenEquipos = "nombre" | "ciudadOrigen";

export interface FiltrosEquipos {
  nombre?: string;
  ciudadOrigen?: string;
  numeroPagina?: number;
  tamanoPagina?: number;
  ordenarPor?: OrdenEquipos;
  direccionOrden?: "asc" | "desc";
}

export async function listarEquipos(filtros?: FiltrosEquipos): Promise<ResultadoPaginado<Equipo>> {
  const parametros = new URLSearchParams({
    numeroPagina: String(filtros?.numeroPagina ?? 1),
    tamanoPagina: String(filtros?.tamanoPagina ?? 10),
  });
  if (filtros?.nombre) parametros.set("nombre", filtros.nombre);
  if (filtros?.ciudadOrigen) parametros.set("ciudadOrigen", filtros.ciudadOrigen);
  if (filtros?.ordenarPor) parametros.set("ordenarPor", filtros.ordenarPor);
  if (filtros?.direccionOrden) parametros.set("direccionOrden", filtros.direccionOrden);

  return obtenerJson<ResultadoPaginado<Equipo>>(`/equipos?${parametros.toString()}`);
}

export async function obtenerEquipo(id: string): Promise<Equipo> {
  return obtenerJson<Equipo>(`/equipos/${id}`);
}

export async function crearEquipo(datos: { nombre: string; ciudadOrigen: string }): Promise<{ id: string }> {
  return enviarJson<{ id: string }>("/equipos", "POST", datos, crypto.randomUUID());
}

export async function actualizarEquipo(id: string, datos: { nombre: string; ciudadOrigen: string }): Promise<void> {
  await enviarJson<void>(`/equipos/${id}`, "PUT", datos);
}

export async function eliminarEquipo(id: string): Promise<void> {
  await enviarJson<void>(`/equipos/${id}`, "DELETE");
}
