import type { ReactNode } from "react";

type Tono = "neutro" | "exito" | "advertencia" | "peligro";

const CLASES_TONO: Record<Tono, string> = {
  neutro: "bg-slate-100 text-slate-700 ring-slate-600/10",
  exito: "bg-emerald-50 text-emerald-700 ring-emerald-600/20",
  advertencia: "bg-amber-50 text-amber-700 ring-amber-600/20",
  peligro: "bg-red-50 text-red-700 ring-red-600/10",
};

type Props = {
  tono?: Tono;
  children: ReactNode;
};

export default function Badge({ tono = "neutro", children }: Props) {
  return (
    <span
      className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ring-1 ring-inset ${CLASES_TONO[tono]}`}
    >
      {children}
    </span>
  );
}
