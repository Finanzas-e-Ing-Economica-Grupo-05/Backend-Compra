namespace backend_finanzas_grupo5.DTOs;

public class SimulacionResponseDto
{
    public int IdSimulacion { get; set; }

    public double MontoTotalFin { get; set; }
    public double MontoCuotaBalon { get; set; }
    public double Tem { get; set; }
    public double CuotaMensual { get; set; }

    public double ValTcea { get; set; }
    public double ValVan { get; set; }
    public double ValTir { get; set; }
    public double MonInteresTot { get; set; }
    public double MonSeguroTot { get; set; }

    public List<CronogramaPagoDto> Cronograma { get; set; } = new();
}