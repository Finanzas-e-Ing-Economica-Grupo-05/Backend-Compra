using Microsoft.EntityFrameworkCore;
using backend_finanzas_grupo5.Data;
using backend_finanzas_grupo5.DTOs;
using backend_finanzas_grupo5.Models;

namespace backend_finanzas_grupo5.Services;

public class SimulacionService : ISimulacionService
{
    private readonly AppDbContext _context;
    private readonly ITasaService _tasaService;
    private readonly IAmortizacionService _amortizacionService;
    private readonly IIndicadoresService _indicadoresService;

    public SimulacionService(
        AppDbContext context,
        ITasaService tasaService,
        IAmortizacionService amortizacionService,
        IIndicadoresService indicadoresService)
    {
        _context = context;
        _tasaService = tasaService;
        _amortizacionService = amortizacionService;
        _indicadoresService = indicadoresService;
    }

    public async Task<SimulacionResponseDto> CrearSimulacionAsync(SimulacionRequestDto request)
    {
        // 1. Conversión de tasas: TasaValor -> TEA -> TEM
        double tea = _tasaService.ConvertirATea(request.TasaValor, request.TasaTipo, request.TasaCapit);
        double tem = _tasaService.ConvertirTeaATem(tea);

        // 2. Cálculo de amortización + cronograma
        var amortizacion = _amortizacionService.Calcular(request, tem);

        // 3. COK mensual para el VAN
        double cokAnual = request.ValCok / 100.0;
        double cokMensual = Math.Pow(1 + cokAnual, 1.0 / 12.0) - 1;

        // 4. Indicadores: VAN, TIR, TCEA
        double van = _indicadoresService.CalcularVan(amortizacion.FlujoCaja, cokMensual);
        double tirMensual = _indicadoresService.CalcularTir(amortizacion.FlujoCaja);
        double tcea = _indicadoresService.CalcularTcea(tirMensual);

        // 5. Persistencia: Simulacion
        var simulacion = new Simulacion
        {
            IdUsuario = request.IdUsuario,
            IdVehiculo = request.IdVehiculo,
            FinFecInicio = request.FinFecInicio,
            FinIniMonto = request.FinIniMonto,
            FinBalonPorc = request.FinBalonPorc,
            FinPlazo = request.FinPlazo,
            TasaValor = request.TasaValor,
            TasaTipo = request.TasaTipo,
            TasaCapit = request.TasaCapit?.ToString(),
            GraTipo = request.GraTipo,
            GraMeses = request.GraMeses,
            SegDesgrav = request.SegDesgrav,
            SegVehic = request.SegVehic,
            ValCok = request.ValCok,
            ComPortes = request.ComPortes
        };

        _context.Simulaciones.Add(simulacion);
        await _context.SaveChangesAsync(); // necesitamos el IdSimulacion generado

        // 6. Persistencia: ResultadoSimulacion
        var resultado = new ResultadoSimulacion
        {
            IdSimulacion = simulacion.IdSimulacion,
            MontoTotalFin = amortizacion.MontoTotalFin,
            MontoCuotaBalon = amortizacion.MontoCuotaBalon,
            Tem = tem,
            ValTcea = tcea,
            ValVan = van,
            ValTir = tirMensual,
            MonInteresTot = amortizacion.MonInteresTot,
            MonSeguroTot = amortizacion.MonSeguroTot
        };
        _context.ResultadosSimulacion.Add(resultado);

        // 7. Persistencia: CronogramaPago (cada mes)
        foreach (var cuota in amortizacion.Cronograma)
        {
            _context.CronogramaPagos.Add(new CronogramaPago
            {
                IdSimulacion = simulacion.IdSimulacion,
                Mes = cuota.Mes,
                TipoCuota = cuota.TipoCuota,
                SaldoInicial = cuota.SaldoInicial,
                AmoPeriodo = cuota.AmoPeriodo,
                IntPeriodo = cuota.IntPeriodo,
                InteresCapitalizado = cuota.InteresCapitalizado,
                SegDesMonto = cuota.SegDesMonto,
                SegVehMonto = cuota.SegVehMonto,
                CuoMensual = cuota.CuoMensual,
                MontoBalonPagado = cuota.MontoBalonPagado,
                PagoTotal = cuota.PagoTotal,
                SaldoFinal = cuota.SaldoFinal
            });
        }

        await _context.SaveChangesAsync();

        // 8. Construir respuesta
        return new SimulacionResponseDto
        {
            IdSimulacion = simulacion.IdSimulacion,
            MontoTotalFin = amortizacion.MontoTotalFin,
            MontoCuotaBalon = amortizacion.MontoCuotaBalon,
            Tem = tem,
            CuotaMensual = amortizacion.CuotaMensual,
            ValTcea = tcea,
            ValVan = van,
            ValTir = tirMensual,
            MonInteresTot = amortizacion.MonInteresTot,
            MonSeguroTot = amortizacion.MonSeguroTot,
            Cronograma = amortizacion.Cronograma
        };
    }

    public async Task<SimulacionResponseDto?> ObtenerPorIdAsync(int idSimulacion)
    {
        var simulacion = await _context.Simulaciones
            .Include(s => s.ResultadoSimulacion)
            .Include(s => s.CronogramaPagos)
            .FirstOrDefaultAsync(s => s.IdSimulacion == idSimulacion);

        if (simulacion == null || simulacion.ResultadoSimulacion == null) return null;

        return new SimulacionResponseDto
        {
            IdSimulacion = simulacion.IdSimulacion,
            MontoTotalFin = simulacion.ResultadoSimulacion.MontoTotalFin,
            MontoCuotaBalon = simulacion.ResultadoSimulacion.MontoCuotaBalon,
            Tem = simulacion.ResultadoSimulacion.Tem,
            CuotaMensual = simulacion.CronogramaPagos.FirstOrDefault()?.CuoMensual ?? 0,
            ValTcea = simulacion.ResultadoSimulacion.ValTcea,
            ValVan = simulacion.ResultadoSimulacion.ValVan,
            ValTir = simulacion.ResultadoSimulacion.ValTir,
            MonInteresTot = simulacion.ResultadoSimulacion.MonInteresTot,
            MonSeguroTot = simulacion.ResultadoSimulacion.MonSeguroTot,
            Cronograma = simulacion.CronogramaPagos
                .OrderBy(c => c.Mes)
                .Select(c => new CronogramaPagoDto
                {
                    Mes = c.Mes,
                    TipoCuota = c.TipoCuota,
                    SaldoInicial = c.SaldoInicial,
                    AmoPeriodo = c.AmoPeriodo,
                    IntPeriodo = c.IntPeriodo,
                    InteresCapitalizado = c.InteresCapitalizado,
                    SegDesMonto = c.SegDesMonto,
                    SegVehMonto = c.SegVehMonto,
                    CuoMensual = c.CuoMensual,
                    MontoBalonPagado = c.MontoBalonPagado,
                    PagoTotal = c.PagoTotal,
                    SaldoFinal = c.SaldoFinal
                }).ToList()
        };
    }

    public async Task<List<SimulacionResponseDto>> ObtenerPorUsuarioAsync(int idUsuario)
    {
        var simulaciones = await _context.Simulaciones
            .Where(s => s.IdUsuario == idUsuario)
            .Include(s => s.ResultadoSimulacion)
            .Include(s => s.Vehiculo)
            .ToListAsync();

        var lista = new List<SimulacionResponseDto>();

        foreach (var s in simulaciones)
        {
            if (s.ResultadoSimulacion == null) continue;

            lista.Add(new SimulacionResponseDto
            {
                IdSimulacion = s.IdSimulacion,
                MontoTotalFin = s.ResultadoSimulacion.MontoTotalFin,
                MontoCuotaBalon = s.ResultadoSimulacion.MontoCuotaBalon,
                Tem = s.ResultadoSimulacion.Tem,
                ValTcea = s.ResultadoSimulacion.ValTcea,
                ValVan = s.ResultadoSimulacion.ValVan,
                ValTir = s.ResultadoSimulacion.ValTir,
                MonInteresTot = s.ResultadoSimulacion.MonInteresTot,
                MonSeguroTot = s.ResultadoSimulacion.MonSeguroTot
            });
        }

        return lista;
    }

    public async Task<bool> EliminarAsync(int idSimulacion)
    {
        var simulacion = await _context.Simulaciones.FindAsync(idSimulacion);
        if (simulacion == null) return false;

        _context.Simulaciones.Remove(simulacion); // cascade borra Resultado y Cronograma
        await _context.SaveChangesAsync();
        return true;
    }
}