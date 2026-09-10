const BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000/api/v1";

export class ErrorApiCliente extends Error {
  codigoError: string;
  traceId: string;

  constructor(codigoError: string, mensaje: string, traceId: string) {
    super(mensaje);
    this.codigoError = codigoError;
    this.traceId = traceId;
  }
}

async function manejarRespuesta<T>(respuesta: Response): Promise<T> {
  if (respuesta.status === 204) {
    return undefined as T;
  }

  const cuerpo = await respuesta.json().catch(() => null);

  if (!respuesta.ok) {
    const error = cuerpo as { codigoError?: string; mensaje?: string; traceId?: string } | null;
    throw new ErrorApiCliente(
      error?.codigoError ?? "ERROR_DESCONOCIDO",
      error?.mensaje ?? "Ocurrió un error inesperado.",
      error?.traceId ?? ""
    );
  }

  return cuerpo as T;
}

export async function obtenerJson<T>(ruta: string): Promise<T> {
  const respuesta = await fetch(`${BASE_URL}${ruta}`);
  return manejarRespuesta<T>(respuesta);
}

export async function enviarJson<T>(
  ruta: string,
  metodo: "POST" | "PUT" | "DELETE",
  cuerpo?: unknown,
  idempotencyKey?: string
): Promise<T> {
  const headers: Record<string, string> = { "Content-Type": "application/json" };
  if (idempotencyKey) {
    headers["Idempotency-Key"] = idempotencyKey;
  }

  const respuesta = await fetch(`${BASE_URL}${ruta}`, {
    method: metodo,
    headers,
    body: cuerpo !== undefined ? JSON.stringify(cuerpo) : undefined,
  });

  return manejarRespuesta<T>(respuesta);
}
