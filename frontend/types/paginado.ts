export interface ResultadoPaginado<T> {
  elementos: T[];
  numeroPagina: number;
  tamanoPagina: number;
  totalRegistros: number;
  totalPaginas: number;
}
