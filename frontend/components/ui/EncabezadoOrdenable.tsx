type Props<T extends string> = {
  columna: T;
  ordenActual: T;
  direccionActual: "asc" | "desc";
  onOrdenar: (columna: T) => void;
  children: React.ReactNode;
};

export default function EncabezadoOrdenable<T extends string>({
  columna,
  ordenActual,
  direccionActual,
  onOrdenar,
  children,
}: Props<T>) {
  const activo = columna === ordenActual;

  return (
    <th scope="col" className="px-3 py-2.5 text-left text-xs font-semibold uppercase tracking-wide text-slate-500">
      <button
        type="button"
        onClick={() => onOrdenar(columna)}
        className={`inline-flex items-center gap-1 rounded transition-colors hover:text-slate-900 ${
          activo ? "text-emerald-700" : ""
        }`}
      >
        {children}
        <span className={`text-[10px] leading-none ${activo ? "text-emerald-600" : "text-slate-300"}`}>
          {activo ? (direccionActual === "asc" ? "▲" : "▼") : "⇅"}
        </span>
      </button>
    </th>
  );
}
