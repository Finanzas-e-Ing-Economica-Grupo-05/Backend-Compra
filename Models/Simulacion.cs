namespace backend_finanzas_grupo5.Models;

public class Simulacion
{
    public int IdSimulacion { get; set; }
    public int IdUsuario { get; set; }
    public int IdVehiculo { get; set; }

    public DateTime FinFecInicio { get; set; }
    public double FinIniMonto { get; set; }
    public double FinBalonPorc { get; set; }
    public int FinPlazo { get; set; }

    public double TasaValor { get; set; }
    public string TasaTipo { get; set; } = string.Empty;   // "Nominal" | "Efectiva"
    public string? TasaCapit { get; set; }                  // frecuencia capitalizacion (si Nominal)

    public string GraTipo { get; set; } = string.Empty;     // "Total" | "Parcial" | "Sin Gracia"
    public int GraMeses { get; set; }

    public double SegDesgrav { get; set; }
    public double SegVehic { get; set; }
    public double ValCok { get; set; }
    public double ComPortes { get; set; }

    public Usuario? Usuario { get; set; }
    public Vehiculo? Vehiculo { get; set; }
    public ResultadoSimulacion? ResultadoSimulacion { get; set; }
    public ICollection<CronogramaPago> CronogramaPagos { get; set; } = new List<CronogramaPago>();
}