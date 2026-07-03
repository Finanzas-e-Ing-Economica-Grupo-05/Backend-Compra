using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_finanzas_grupo5.Data;
using backend_finanzas_grupo5.DTOs;
using backend_finanzas_grupo5.Models;
using BCrypt.Net;

namespace backend_finanzas_grupo5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        bool existe = await _context.Usuarios
            .AnyAsync(u => u.UsuCorreo == dto.UsuCorreo || u.UsuDni == dto.UsuDni);

        if (existe)
            return BadRequest("Ya existe un usuario con ese correo o DNI.");

        // Hasheamos la contraseña antes de guardar
        string passwordHasheado = BCrypt.Net.BCrypt.HashPassword(dto.UsuPassword);

        var usuario = new Usuario
        {
            UsuNombre = dto.UsuNombre,
            UsuApellido = dto.UsuApellido,
            UsuDni = dto.UsuDni,
            UsuPassword = passwordHasheado,
            UsuCorreo = dto.UsuCorreo,
            UsuNumero = dto.UsuNumero,
            UsuPerfilSe = dto.UsuPerfilSe
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { usuario.IdUsuario, usuario.UsuNombre, usuario.UsuCorreo });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.UsuCorreo == dto.UsuCorreo);

        if (usuario == null)
            return Unauthorized("Credenciales inválidas.");

        // Verificamos el password ingresado contra el hash guardado
        bool passwordValido = BCrypt.Net.BCrypt.Verify(dto.UsuPassword, usuario.UsuPassword);

        if (!passwordValido)
            return Unauthorized("Credenciales inválidas.");

        return Ok(new
        {
            usuario.IdUsuario,
            usuario.UsuNombre,
            usuario.UsuApellido,
            usuario.UsuCorreo
        });
    }
}