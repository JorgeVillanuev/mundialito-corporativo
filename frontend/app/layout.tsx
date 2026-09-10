import type { Metadata } from "next";
import NavegacionPrincipal from "@/components/NavegacionPrincipal";
import "./globals.css";

export const metadata: Metadata = {
  title: "Mundialito de Fútbol Corporativo",
  description: "Sistema de gestión del torneo",
};

export default function RootLayout({ children }: LayoutProps<"/">) {
  return (
    <html lang="es">
      <body>
        <NavegacionPrincipal />
        <main className="mx-auto max-w-6xl px-6 py-8">{children}</main>
      </body>
    </html>
  );
}
