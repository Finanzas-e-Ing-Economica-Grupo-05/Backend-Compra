using backend_finanzas_grupo5.DTOs;

namespace backend_finanzas_grupo5.Services;

public interface ISimulacionService
{
    Task<SimulacionResponseDto> CrearSimulacionAsync(SimulacionRequestDto request);
    Task<SimulacionResponseDto?> ObtenerPorIdAsync(int idSimulacion);
    Task<List<SimulacionResponseDto>> ObtenerPorUsuarioAsync(int idUsuario);
    Task<bool> EliminarAsync(int idSimulacion);
}