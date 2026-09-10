export type EstadoPartido = "Programado" | "Jugado";

export interface Partido {
  id: string;
  equipoLocalId: string;
  nombreEquipoLocal: string;
  equipoVisitanteId: string;
  nombreEquipoVisitante: string;
  fechaHora: string;
  estado: EstadoPartido;
  golesLocal: number | null;
  golesVisitante: number | null;
}
