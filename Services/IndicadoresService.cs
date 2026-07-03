namespace backend_finanzas_grupo5.Services;

public class IndicadoresService : IIndicadoresService
{
    public double CalcularVan(List<double> flujoCaja, double tasaDescuentoMensual)
    {
        // flujoCaja[0] es el desembolso (positivo), flujoCaja[1..n] son pagos (negativos)
        double van = flujoCaja[0];

        for (int t = 1; t < flujoCaja.Count; t++)
        {
            double factorDescuento = Math.Pow(1 + tasaDescuentoMensual, t);
            van += flujoCaja[t] / factorDescuento;
        }

        return van;
    }

    public double CalcularTir(List<double> flujoCaja)
    {
        double limiteInf = 0.0;
        double limiteSup = 1.0;
        double tirCalc = 0.0;

        for (int iteracion = 1; iteracion <= 100; iteracion++)
        {
            tirCalc = (limiteInf + limiteSup) / 2.0;
            double vanPrueba = CalcularVan(flujoCaja, tirCalc);

            if (vanPrueba > 0)
                limiteSup = tirCalc;
            else
                limiteInf = tirCalc;
        }

        return tirCalc;
    }

    public double CalcularTcea(double tirMensual)
    {
        return Math.Pow(1 + tirMensual, 12) - 1;
    }
}