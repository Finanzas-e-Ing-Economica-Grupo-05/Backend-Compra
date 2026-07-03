namespace backend_finanzas_grupo5.Services;

public interface IIndicadoresService
{
    double CalcularVan(List<double> flujoCaja, double tasaDescuentoMensual);
    double CalcularTir(List<double> flujoCaja);
    double CalcularTcea(double tirMensual);
}