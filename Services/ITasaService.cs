namespace backend_finanzas_grupo5.Services;

public interface ITasaService
{
    double ConvertirATea(double tasaValor, string tasaTipo, int? tasaCapitDias);
    double ConvertirTeaATem(double tea);
}