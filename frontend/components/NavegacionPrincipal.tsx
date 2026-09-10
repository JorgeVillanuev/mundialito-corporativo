"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

const ENLACES = [
  { href: "/equipos", etiqueta: "Equipos" },
  { href: "/partidos", etiqueta: "Partidos" },
  { href: "/posiciones", etiqueta: "Posiciones" },
  { href: "/goleadores", etiqueta: "Goleadores" },
];

export default function NavegacionPrincipal() {
  const pathname = usePathname();

  return (
    <header className="border-b border-slate-200 bg-white">
      <div className="mx-auto flex max-w-6xl items-center gap-6 px-6 py-3.5">
        <Link href="/equipos" className="flex items-center gap-2 text-base font-semibold text-slate-800">
          <span aria-hidden>⚽</span>
          Mundialito Corporativo
        </Link>
        <nav className="flex items-center gap-1">
          {ENLACES.map((enlace) => {
            const activo = pathname?.startsWith(enlace.href);
            return (
              <Link
                key={enlace.href}
                href={enlace.href}
                className={`rounded-md px-3 py-1.5 text-sm font-medium transition-colors ${
                  activo ? "bg-emerald-50 text-emerald-700" : "text-slate-600 hover:bg-slate-100"
                }`}
              >
                {enlace.etiqueta}
              </Link>
            );
          })}
        </nav>
      </div>
    </header>
  );
}
