namespace backend_finanzas_grupo5.Models;

public class Vehiculo
{
    public int IdVehiculo { get; set; }
    public string VehMarca { get; set; } = string.Empty;
    public string? VehModelo { get; set; }
    public double VehPrecio { get; set; }
    public string VehMoneda { get; set; } = string.Empty;
    public string? VehImagenUrl { get; set; }

    public ICollection<Simulacion> Simulaciones { get; set; } = new List<Simulacion>();
}