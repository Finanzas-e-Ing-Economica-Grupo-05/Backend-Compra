namespace backend_finanzas_grupo5.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string UsuNombre { get; set; } = string.Empty;
    public string UsuApellido { get; set; } = string.Empty;
    public string UsuDni { get; set; } = string.Empty;
    public string UsuPassword { get; set; } = string.Empty;
    public string UsuCorreo { get; set; } = string.Empty;
    public string UsuNumero { get; set; } = string.Empty;
    public double UsuPerfilSe { get; set; }

    public ICollection<Simulacion> Simulaciones { get; set; } = new List<Simulacion>();
}