namespace backend_finanzas_grupo5.Services;

public class TasaService : ITasaService
{
    public double ConvertirATea(double tasaValor, string tasaTipo, int? tasaCapitDias)
    {
        double tasaDecimal = tasaValor / 100.0;

        if (tasaTipo.Equals("Nominal", StringComparison.OrdinalIgnoreCase))
        {
            int capitDias = tasaCapitDias ?? 30;
            double m = 360.0 / capitDias;
            return Math.Pow(1 + (tasaDecimal / m), m) - 1;
        }

        // Si ya es Efectiva (TEA), se devuelve tal cual
        return tasaDecimal;
    }

    public double ConvertirTeaATem(double tea)
    {
        return Math.Pow(1 + tea, 1.0 / 12.0) - 1;
    }
}