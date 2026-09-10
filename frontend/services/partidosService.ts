import { obtenerJson, enviarJson } from "./apiClient";
import type { EstadoPartido, Partido } from "@/types/partido";
import type { ResultadoPaginado } from "@/types/paginado";

export type OrdenPartidos = "fechaHora" | "estado";

export interface FiltrosPartidos {
  fechaDesde?: string;
  fechaHasta?: string;
  equipoId?: string;
  estado?: EstadoPartido;
  numeroPagina?: number;
  tamanoPagina?: number;
  ordenarPor?: OrdenPartidos;
  direccionOrden?: "asc" | "desc";
}

export async function listarPartidos(filtros?: FiltrosPartidos): Promise<ResultadoPaginado<Partido>> {
  const parametros = new URLSearchParams({
    numeroPagina: String(filtros?.numeroPagina ?? 1),
    tamanoPagina: String(filtros?.tamanoPagina ?? 10),
  });
  if (filtros?.fechaDesde) parametros.set("fechaDesde", filtros.fechaDesde);
  if (filtros?.fechaHasta) parametros.set("fechaHasta", filtros.fechaHasta);
  if (filtros?.equipoId) parametros.set("equipoId", filtros.equipoId);
  if (filtros?.estado) parametros.set("estado", filtros.estado);
  if (filtros?.ordenarPor) parametros.set("ordenarPor", filtros.ordenarPor);
  if (filtros?.direccionOrden) parametros.set("direccionOrden", filtros.direccionOrden);

  return obtenerJson<ResultadoPaginado<Partido>>(`/partidos?${parametros.toString()}`);
}

export async function crearPartido(datos: {
  equipoLocalId: string;
  equipoVisitanteId: string;
  fechaHora: string;
}): Promise<{ id: string }> {
  return enviarJson<{ id: string }>("/partidos", "POST", datos, crypto.randomUUID());
}

export async function registrarResultado(
  partidoId: string,
  datos: {
    golesLocal: number;
    golesVisitante: number;
    goleadores: { jugadorId: string; cantidadGoles: number }[];
  }
): Promise<void> {
  await enviarJson<void>(`/partidos/${partidoId}/resultado`, "PUT", datos);
}
