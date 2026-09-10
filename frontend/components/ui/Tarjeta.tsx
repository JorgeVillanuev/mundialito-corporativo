import type { ReactNode } from "react";

type Props = {
  titulo?: string;
  children: ReactNode;
  className?: string;
};

export default function Tarjeta({ titulo, children, className = "" }: Props) {
  return (
    <section className={`rounded-xl border border-slate-200 bg-white p-5 shadow-sm ${className}`}>
      {titulo && <h2 className="mb-4 text-base font-semibold text-slate-800">{titulo}</h2>}
      {children}
    </section>
  );
}
