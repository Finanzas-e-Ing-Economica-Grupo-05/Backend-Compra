using Microsoft.AspNetCore.Mvc;
using backend_finanzas_grupo5.DTOs;
using backend_finanzas_grupo5.Services;

namespace backend_finanzas_grupo5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulacionController : ControllerBase
{
    private readonly ISimulacionService _simulacionService;

    public SimulacionController(ISimulacionService simulacionService)
    {
        _simulacionService = simulacionService;
    }

    [HttpPost]
    public async Task<ActionResult<SimulacionResponseDto>> Crear([FromBody] SimulacionRequestDto request)
    {
        if (request.FinBalonPorc < 20 || request.FinBalonPorc > 50)
            return BadRequest("El porcentaje de cuota balón debe estar entre 20% y 50%.");

        if (request.IdVehiculo <= 0)
            return BadRequest("Debes seleccionar un vehículo válido antes de simular.");

        if (request.FinPlazo <= 0)
            return BadRequest("El plazo debe ser mayor a 0 meses.");

        if (request.GraMeses < 0 || request.GraMeses >= request.FinPlazo)
            return BadRequest("Los meses de gracia deben ser mayores o iguales a 0 y menores al plazo total.");

        var tiposGraciaValidos = new[] { "Total", "Parcial", "Sin Gracia" };
        if (!tiposGraciaValidos.Contains(request.GraTipo, StringComparer.OrdinalIgnoreCase))
            return BadRequest("Tipo de gracia inválido. Debe ser 'Total', 'Parcial' o 'Sin Gracia'.");

        var tiposTasaValidos = new[] { "Nominal", "Efectiva" };
        if (!tiposTasaValidos.Contains(request.TasaTipo, StringComparer.OrdinalIgnoreCase))
            return BadRequest("Tipo de tasa inválido. Debe ser 'Nominal' o 'Efectiva'.");

        if (request.TasaTipo.Equals("Nominal", StringComparison.OrdinalIgnoreCase))
        {
            var capitalizacionesValidas = new[] { 1, 30, 90, 180, 360 };
            if (request.TasaCapit == null || !capitalizacionesValidas.Contains(request.TasaCapit.Value))
                return BadRequest("Frecuencia de capitalización inválida. Debe ser 1, 30, 90, 180 o 360 días.");
        }

        // Regla del marco conceptual: 0 <= CB <= SF
        double saldoFinanciado = request.VehPrecio - request.FinIniMonto;
        double cuotaBalon = request.VehPrecio * (request.FinBalonPorc / 100.0);
        if (saldoFinanciado <= 0)
            return BadRequest("La cuota inicial no puede ser mayor o igual al precio del vehículo.");

        if (cuotaBalon > saldoFinanciado)
            return BadRequest("La cuota balón no puede ser mayor al monto financiado. Aumenta la cuota inicial o reduce el % de balón.");

        if (request.TasaValor <= 0)
            return BadRequest("La tasa de interés debe ser mayor a 0.");

        if (request.SegDesgrav < 0 || request.SegVehic < 0 || request.ComPortes < 0)
            return BadRequest("Los seguros y portes no pueden ser negativos.");

        try
        {
            var resultado = await _simulacionService.CrearSimulacionAsync(request);
            return Ok(resultado);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            return BadRequest("El usuario o vehículo indicado no existe en la base de datos.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SimulacionResponseDto>> ObtenerPorId(int id)
    {
        var resultado = await _simulacionService.ObtenerPorIdAsync(id);
        if (resultado == null) return NotFound();
        return Ok(resultado);
    }

    [HttpGet("usuario/{idUsuario}")]
    public async Task<ActionResult<List<SimulacionResponseDto>>> ObtenerPorUsuario(int idUsuario)
    {
        var resultado = await _simulacionService.ObtenerPorUsuarioAsync(idUsuario);
        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ok = await _simulacionService.EliminarAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}