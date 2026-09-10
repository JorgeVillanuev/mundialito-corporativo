"use client";

import { useMemo, useState, type FormEvent } from "react";
import type { Jugador } from "@/types/jugador";
import Boton from "@/components/ui/Boton";
import Input from "@/components/ui/Input";
import Select from "@/components/ui/Select";
import Campo from "@/components/ui/Campo";

type GoleadorEntrada = { jugadorId: string; cantidadGoles: number };

type Props = {
  jugadoresLocal: Jugador[];
  jugadoresVisitante: Jugador[];
  onRegistrar: (datos: {
    golesLocal: number;
    golesVisitante: number;
    goleadores: GoleadorEntrada[];
  }) => Promise<void>;
};

export default function FormularioResultado({ jugadoresLocal, jugadoresVisitante, onRegistrar }: Props) {
  const [goleadores, setGoleadores] = useState<GoleadorEntrada[]>([]);
  const [enviando, setEnviando] = useState(false);

  const todosJugadores = [...jugadoresLocal, ...jugadoresVisitante];
  const idsJugadoresLocal = useMemo(() => new Set(jugadoresLocal.map((j) => j.id)), [jugadoresLocal]);

  const golesLocal = goleadores
    .filter((g) => idsJugadoresLocal.has(g.jugadorId))
    .reduce((total, g) => total + g.cantidadGoles, 0);
  const golesVisitante = goleadores
    .filter((g) => !idsJugadoresLocal.has(g.jugadorId))
    .reduce((total, g) => total + g.cantidadGoles, 0);

  function agregarGoleador() {
    if (todosJugadores.length === 0) return;
    setGoleadores([...goleadores, { jugadorId: todosJugadores[0].id, cantidadGoles: 1 }]);
  }

  function actualizarGoleador(indice: number, campo: keyof GoleadorEntrada, valor: string) {
    const copia = [...goleadores];
    copia[indice] = {
      ...copia[indice],
      [campo]: campo === "cantidadGoles" ? Number(valor) : valor,
    };
    setGoleadores(copia);
  }

  function quitarGoleador(indice: number) {
    setGoleadores(goleadores.filter((_, i) => i !== indice));
  }

  async function manejarEnvio(evento: FormEvent) {
    evento.preventDefault();
    setEnviando(true);
    try {
      await onRegistrar({ golesLocal, golesVisitante, goleadores });
    } finally {
      setEnviando(false);
    }
  }

  return (
    <form onSubmit={manejarEnvio} className="flex flex-col gap-4">
      <div className="grid grid-cols-2 gap-3">
        <Campo etiqueta="Goles local">
          <Input type="number" value={golesLocal} readOnly className="bg-slate-50 text-slate-500" />
        </Campo>
        <Campo etiqueta="Goles visitante">
          <Input type="number" value={golesVisitante} readOnly className="bg-slate-50 text-slate-500" />
        </Campo>
      </div>
      <p className="-mt-2 text-xs text-slate-500">El marcador se calcula solo, sumando los goleadores de abajo.</p>

      <div>
        <h4 className="mb-2 text-sm font-semibold text-slate-700">Goleadores</h4>
        <div className="flex flex-col gap-2">
          {goleadores.map((goleador, indice) => (
            <div key={indice} className="flex items-center gap-2">
              <Select
                className="flex-1"
                value={goleador.jugadorId}
                onChange={(e) => actualizarGoleador(indice, "jugadorId", e.target.value)}
              >
                {todosJugadores.map((jugador) => (
                  <option key={jugador.id} value={jugador.id}>
                    {jugador.nombre}
                  </option>
                ))}
              </Select>
              <Input
                type="number"
                min={1}
                className="w-20! shrink-0"
                value={goleador.cantidadGoles}
                onChange={(e) => actualizarGoleador(indice, "cantidadGoles", e.target.value)}
              />
              <Boton type="button" variante="peligro" onClick={() => quitarGoleador(indice)}>
                Quitar
              </Boton>
            </div>
          ))}
        </div>
        <Boton
          type="button"
          variante="secundario"
          className="mt-2"
          onClick={agregarGoleador}
          disabled={todosJugadores.length === 0}
        >
          + Agregar goleador
        </Boton>
      </div>

      <div>
        <Boton type="submit" variante="primario" disabled={enviando}>
          {enviando ? "Guardando..." : "Registrar resultado"}
        </Boton>
      </div>
    </form>
  );
}
