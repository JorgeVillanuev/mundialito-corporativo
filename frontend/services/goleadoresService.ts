import { obtenerJson } from "./apiClient";
import type { Goleador } from "@/types/goleador";
import type { ResultadoPaginado } from "@/types/paginado";

export type OrdenGoleadores = "cantidadGoles" | "nombreJugador";

export interface FiltrosGoleadores {
  equipoId?: string;
  numeroPagina?: number;
  tamanoPagina?: number;
  ordenarPor?: OrdenGoleadores;
  direccionOrden?: "asc" | "desc";
}

export async function obtenerGoleadores(filtros?: FiltrosGoleadores): Promise<ResultadoPaginado<Goleador>> {
  const parametros = new URLSearchParams({
    numeroPagina: String(filtros?.numeroPagina ?? 1),
    tamanoPagina: String(filtros?.tamanoPagina ?? 10),
  });
  if (filtros?.equipoId) parametros.set("equipoId", filtros.equipoId);
  if (filtros?.ordenarPor) parametros.set("ordenarPor", filtros.ordenarPor);
  if (filtros?.direccionOrden) parametros.set("direccionOrden", filtros.direccionOrden);

  return obtenerJson<ResultadoPaginado<Goleador>>(`/goleadores?${parametros.toString()}`);
}
