namespace backend_finanzas_grupo5.DTOs;

public class CronogramaPagoDto
{
    public int Mes { get; set; }
    public string TipoCuota { get; set; } = string.Empty;
    public double SaldoInicial { get; set; }
    public double AmoPeriodo { get; set; }
    public double IntPeriodo { get; set; }
    public double? InteresCapitalizado { get; set; }
    public double SegDesMonto { get; set; }
    public double SegVehMonto { get; set; }
    public double CuoMensual { get; set; }
    public double? MontoBalonPagado { get; set; }
    public double PagoTotal { get; set; }
    public double SaldoFinal { get; set; }
}