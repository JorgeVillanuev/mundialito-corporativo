import type { ReactNode } from "react";

type Props = {
  etiqueta: string;
  children: ReactNode;
  className?: string;
};

export default function Campo({ etiqueta, children, className = "" }: Props) {
  return (
    <label className={`flex flex-col gap-1 text-sm ${className}`}>
      <span className="font-medium text-slate-600">{etiqueta}</span>
      {children}
    </label>
  );
}
