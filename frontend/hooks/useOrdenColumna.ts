import { useState } from "react";

type Direccion = "asc" | "desc";

export function useOrdenColumna<T extends string>(
  columnaInicial: T,
  direccionInicial: Direccion = "asc",
  direccionPorColumna?: Partial<Record<T, Direccion>>
) {
  const [ordenarPor, setOrdenarPor] = useState<T>(columnaInicial);
  const [direccionOrden, setDireccionOrden] = useState<Direccion>(direccionInicial);

  function ordenarPorColumna(columna: T) {
    if (columna === ordenarPor) {
      setDireccionOrden((d) => (d === "asc" ? "desc" : "asc"));
    } else {
      setOrdenarPor(columna);
      setDireccionOrden(direccionPorColumna?.[columna] ?? "asc");
    }
  }

  return { ordenarPor, direccionOrden, ordenarPorColumna };
}
