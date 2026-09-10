import Link from "next/link";
import Tarjeta from "@/components/ui/Tarjeta";

const SECCIONES = [
  { href: "/equipos", titulo: "Equipos", descripcion: "Alta, edición y roster de jugadores.", icono: "🛡️" },
  { href: "/partidos", titulo: "Partidos", descripcion: "Programación y registro de resultados.", icono: "📅" },
  { href: "/posiciones", titulo: "Tabla de posiciones", descripcion: "Posiciones del torneo.", icono: "🏆" },
  { href: "/goleadores", titulo: "Goleadores", descripcion: "Ranking de goleadores.", icono: "⚽" },
];

export default function InicioPage() {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-900">Mundialito de Fútbol Corporativo</h1>
        <p className="mt-1 text-sm text-slate-500">Sistema de gestión del torneo.</p>
      </div>

      <div className="grid gap-4 sm:grid-cols-2">
        {SECCIONES.map((seccion) => (
          <Link key={seccion.href} href={seccion.href}>
            <Tarjeta className="h-full transition-shadow hover:shadow-md">
              <div className="flex items-start gap-3">
                <span className="text-2xl" aria-hidden>
                  {seccion.icono}
                </span>
                <div>
                  <h2 className="font-semibold text-slate-800">{seccion.titulo}</h2>
                  <p className="mt-0.5 text-sm text-slate-500">{seccion.descripcion}</p>
                </div>
              </div>
            </Tarjeta>
          </Link>
        ))}
      </div>
    </div>
  );
}
