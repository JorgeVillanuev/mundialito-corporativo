type Props = {
  mensaje: string | null;
};

export default function MensajeError({ mensaje }: Props) {
  if (!mensaje) return null;

  return (
    <div className="flex items-start gap-2 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
      <span aria-hidden className="mt-0.5">
        ⚠
      </span>
      <p>{mensaje}</p>
    </div>
  );
}
