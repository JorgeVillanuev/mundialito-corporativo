import { useState } from "react";

export function usePaginacion(tamanoPaginaInicial = 10) {
  const [numeroPagina, setNumeroPagina] = useState(1);
  const [tamanoPagina, setTamanoPagina] = useState(tamanoPaginaInicial);
  const [totalPaginas, setTotalPaginas] = useState(1);
  const [totalRegistros, setTotalRegistros] = useState(0);

  function cambiarTamanoPagina(tamano: number) {
    setTamanoPagina(tamano);
    setNumeroPagina(1);
  }

  function reiniciar() {
    setNumeroPagina(1);
  }

  function actualizarDesdeResultado(resultado: { totalPaginas: number; totalRegistros: number }) {
    setTotalPaginas(resultado.totalPaginas);
    setTotalRegistros(resultado.totalRegistros);
  }

  return {
    numeroPagina,
    tamanoPagina,
    totalPaginas,
    totalRegistros,
    setNumeroPagina,
    cambiarTamanoPagina,
    reiniciar,
    actualizarDesdeResultado,
  };
}
