using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_finanzas_grupo5.Data;
using backend_finanzas_grupo5.Models;

namespace backend_finanzas_grupo5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiculoController : ControllerBase
{
    private readonly AppDbContext _context;

    public VehiculoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Vehiculo>>> ObtenerTodos()
    {
        return Ok(await _context.Vehiculos.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vehiculo>> ObtenerPorId(int id)
    {
        var vehiculo = await _context.Vehiculos.FindAsync(id);
        if (vehiculo == null) return NotFound();
        return Ok(vehiculo);
    }

    [HttpPost]
    public async Task<ActionResult<Vehiculo>> Crear([FromBody] Vehiculo vehiculo)
    {
        _context.Vehiculos.Add(vehiculo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(ObtenerPorId), new { id = vehiculo.IdVehiculo }, vehiculo);
    }

    [HttpPost("lote")]
    public async Task<ActionResult<List<Vehiculo>>> CrearVarios([FromBody] List<Vehiculo> vehiculos)
    {
        _context.Vehiculos.AddRange(vehiculos);
        await _context.SaveChangesAsync();
        return Ok(vehiculos);
    }
}