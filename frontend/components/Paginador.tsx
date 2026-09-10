const OPCIONES_TAMANO_DEFECTO = [5, 10, 25, 50];

type Props = {
  numeroPagina: number;
  totalPaginas: number;
  onCambiarPagina: (pagina: number) => void;
  tamanoPagina?: number;
  onCambiarTamanoPagina?: (tamano: number) => void;
  opcionesTamano?: number[];
};

export default function Paginador({
  numeroPagina,
  totalPaginas,
  onCambiarPagina,
  tamanoPagina,
  onCambiarTamanoPagina,
  opcionesTamano = OPCIONES_TAMANO_DEFECTO,
}: Props) {
  const mostrarSelectorTamano = tamanoPagina !== undefined && onCambiarTamanoPagina !== undefined;

  if (totalPaginas <= 1 && !mostrarSelectorTamano) return null;

  const CLASE_BOTON =
    "inline-flex h-8 min-w-8 items-center justify-center rounded-md px-2 text-sm font-medium " +
    "transition-colors disabled:cursor-not-allowed disabled:opacity-40 text-slate-600 hover:bg-slate-100";

  return (
    <div className="mt-4 flex flex-wrap items-center justify-between gap-3 border-t border-slate-100 pt-4">
      {mostrarSelectorTamano ? (
        <label className="flex items-center gap-2 text-sm text-slate-500">
          Mostrar
          <select
            className="rounded-md border border-slate-300 bg-white px-2 py-1 text-sm text-slate-700 focus:border-emerald-500 focus:outline focus:outline-2 focus:outline-emerald-500/30"
            value={tamanoPagina}
            onChange={(e) => onCambiarTamanoPagina!(Number(e.target.value))}
          >
            {opcionesTamano.map((opcion) => (
              <option key={opcion} value={opcion}>
                {opcion}
              </option>
            ))}
          </select>
          por página
        </label>
      ) : (
        <span />
      )}

      {totalPaginas > 1 && (
        <div className="flex items-center gap-3">
          <button className={CLASE_BOTON} disabled={numeroPagina <= 1} onClick={() => onCambiarPagina(numeroPagina - 1)}>
            ← Anterior
          </button>
          <span className="text-sm text-slate-500">
            Página <span className="font-semibold text-slate-700">{numeroPagina}</span> de{" "}
            <span className="font-semibold text-slate-700">{totalPaginas}</span>
          </span>
          <button
            className={CLASE_BOTON}
            disabled={numeroPagina >= totalPaginas}
            onClick={() => onCambiarPagina(numeroPagina + 1)}
          >
            Siguiente →
          </button>
        </div>
      )}
    </div>
  );
}
