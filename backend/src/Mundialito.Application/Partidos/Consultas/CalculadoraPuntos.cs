namespace Mundialito.Application.Partidos.Consultas;

public static class CalculadoraPuntos
{
    public static int Calcular(int victorias, int empates) => victorias * 3 + empates;
}
