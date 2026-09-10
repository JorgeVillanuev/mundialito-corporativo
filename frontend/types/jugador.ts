export type PosicionJugador = "Arquero" | "Defensor" | "Mediocampista" | "Delantero";

export const POSICIONES_JUGADOR: PosicionJugador[] = [
  "Arquero",
  "Defensor",
  "Mediocampista",
  "Delantero",
];

export interface Jugador {
  id: string;
  equipoId: string;
  nombre: string;
  posicion: PosicionJugador;
}
