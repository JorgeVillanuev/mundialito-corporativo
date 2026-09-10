import { obtenerJson } from "./apiClient";
import type { Posicion } from "@/types/posicion";
import type { ResultadoPaginado } from "@/types/paginado";

export async function obtenerPosiciones(numeroPagina = 1, tamanoPagina = 10): Promise<ResultadoPaginado<Posicion>> {
  const parametros = new URLSearchParams({
    numeroPagina: String(numeroPagina),
    tamanoPagina: String(tamanoPagina),
  });

  return obtenerJson<ResultadoPaginado<Posicion>>(`/posiciones?${parametros.toString()}`);
}
