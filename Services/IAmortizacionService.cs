using backend_finanzas_grupo5.DTOs;

namespace backend_finanzas_grupo5.Services;

public class AmortizacionResult
{
    public double MontoTotalFin { get; set; }
    public double MontoCuotaBalon { get; set; }
    public double Tem { get; set; }
    public double CuotaMensual { get; set; }
    public double MonInteresTot { get; set; }
    public double MonSeguroTot { get; set; }
    public List<CronogramaPagoDto> Cronograma { get; set; } = new();
    public List<double> FlujoCaja { get; set; } = new(); // posicion 0 = desembolso (+), 1..N pagos (-)
}

public interface IAmortizacionService
{
    AmortizacionResult Calcular(SimulacionRequestDto request, double tem);
}