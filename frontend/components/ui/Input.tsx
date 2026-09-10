import type { InputHTMLAttributes } from "react";

type Props = InputHTMLAttributes<HTMLInputElement>;

export default function Input({ className = "", ...resto }: Props) {
  return (
    <input
      className={
        "w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 " +
        "placeholder:text-slate-400 focus:border-emerald-500 focus:outline focus:outline-2 " +
        "focus:outline-emerald-500/30 " +
        className
      }
      {...resto}
    />
  );
}
