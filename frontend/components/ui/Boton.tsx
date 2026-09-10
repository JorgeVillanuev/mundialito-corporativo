import type { ButtonHTMLAttributes } from "react";

type Variante = "primario" | "secundario" | "peligro" | "fantasma";

const CLASES_BASE =
  "inline-flex items-center justify-center gap-1.5 rounded-lg px-3.5 py-2 text-sm font-medium " +
  "transition-colors disabled:cursor-not-allowed disabled:opacity-50 focus-visible:outline " +
  "focus-visible:outline-2 focus-visible:outline-offset-2";

const CLASES_VARIANTE: Record<Variante, string> = {
  primario: "bg-emerald-600 text-white shadow-sm hover:bg-emerald-700 focus-visible:outline-emerald-600",
  secundario:
    "bg-white text-slate-700 ring-1 ring-inset ring-slate-300 hover:bg-slate-50 focus-visible:outline-slate-400",
  peligro: "bg-white text-red-600 ring-1 ring-inset ring-red-200 hover:bg-red-50 focus-visible:outline-red-500",
  fantasma: "text-slate-600 hover:bg-slate-100 focus-visible:outline-slate-400",
};

type Props = ButtonHTMLAttributes<HTMLButtonElement> & {
  variante?: Variante;
};

export default function Boton({ variante = "secundario", className = "", ...resto }: Props) {
  return <button className={`${CLASES_BASE} ${CLASES_VARIANTE[variante]} ${className}`} {...resto} />;
}
