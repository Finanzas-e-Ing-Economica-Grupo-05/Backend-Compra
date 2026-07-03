namespace backend_finanzas_grupo5.DTOs;

public class SimulacionRequestDto
{
    public int IdUsuario { get; set; }
    public int IdVehiculo { get; set; }

    public double VehPrecio { get; set; }     // por si no viene de catálogo, se ingresa manual
    public string VehMoneda { get; set; } = "Soles";

    public DateTime FinFecInicio { get; set; }
    public double FinIniMonto { get; set; }
    public double FinBalonPorc { get; set; }   // ej: 30 (representa 30%)
    public int FinPlazo { get; set; }

    public double TasaValor { get; set; }      // ej: 12.5 (representa 12.5%)
    public string TasaTipo { get; set; } = "Efectiva";   // "Nominal" | "Efectiva"
    public int? TasaCapit { get; set; }        // dias: 1,30,90,180,360 (solo si Nominal)

    public string GraTipo { get; set; } = "Sin Gracia";  // "Total" | "Parcial" | "Sin Gracia"
    public int GraMeses { get; set; }

    public double SegDesgrav { get; set; }     // % mensual
    public double SegVehic { get; set; }       // % anual
    public double ValCok { get; set; }         // % anual
    public double ComPortes { get; set; }
}