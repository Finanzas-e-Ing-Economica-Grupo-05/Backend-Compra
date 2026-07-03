namespace backend_finanzas_grupo5.DTOs;

public class LoginRequestDto
{
    public string UsuCorreo { get; set; } = string.Empty;
    public string UsuPassword { get; set; } = string.Empty;
}