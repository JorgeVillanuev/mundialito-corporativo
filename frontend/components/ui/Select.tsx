import type { SelectHTMLAttributes } from "react";

type Props = SelectHTMLAttributes<HTMLSelectElement>;

export default function Select({ className = "", ...resto }: Props) {
  return (
    <select
      className={
        "w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 " +
        "focus:border-emerald-500 focus:outline focus:outline-2 focus:outline-emerald-500/30 " +
        className
      }
      {...resto}
    />
  );
}
