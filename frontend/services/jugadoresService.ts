import { obtenerJson, enviarJson } from "./apiClient";
import type { Jugador, PosicionJugador } from "@/types/jugador";
import type { ResultadoPaginado } from "@/types/paginado";

export type OrdenJugadores = "nombre" | "posicion";

export interface FiltrosJugadores {
  nombre?: string;
  posicion?: PosicionJugador;
  numeroPagina?: number;
  tamanoPagina?: number;
  ordenarPor?: OrdenJugadores;
  direccionOrden?: "asc" | "desc";
}

export async function listarJugadoresDeEquipo(
  equipoId: string,
  filtros?: FiltrosJugadores
): Promise<ResultadoPaginado<Jugador>> {
  const parametros = new URLSearchParams({
    numeroPagina: String(filtros?.numeroPagina ?? 1),
    tamanoPagina: String(filtros?.tamanoPagina ?? 10),
  });
  if (filtros?.nombre) parametros.set("nombre", filtros.nombre);
  if (filtros?.posicion) parametros.set("posicion", filtros.posicion);
  if (filtros?.ordenarPor) parametros.set("ordenarPor", filtros.ordenarPor);
  if (filtros?.direccionOrden) parametros.set("direccionOrden", filtros.direccionOrden);

  return obtenerJson<ResultadoPaginado<Jugador>>(`/equipos/${equipoId}/jugadores?${parametros.toString()}`);
}

export async function registrarJugador(
  equipoId: string,
  datos: { nombre: string; posicion: PosicionJugador }
): Promise<{ id: string }> {
  return enviarJson<{ id: string }>(`/equipos/${equipoId}/jugadores`, "POST", datos, crypto.randomUUID());
}
