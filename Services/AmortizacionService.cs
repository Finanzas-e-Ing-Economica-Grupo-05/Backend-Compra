using backend_finanzas_grupo5.DTOs;

namespace backend_finanzas_grupo5.Services;

public class AmortizacionService : IAmortizacionService
{
    public AmortizacionResult Calcular(SimulacionRequestDto request, double tem)
    {
        var result = new AmortizacionResult { Tem = tem };

        double montoTotalFin = request.VehPrecio - request.FinIniMonto;
        double montoCuotaBalon = request.VehPrecio * (request.FinBalonPorc / 100.0);
        double segVehMensual = (request.VehPrecio * (request.SegVehic / 100.0)) / 12.0;

        int n = request.FinPlazo;
        int g = request.GraMeses;
        int plazoEfectivo = n - g;

        string graTipo = request.GraTipo;

        // Proyección del saldo si hay gracia total (para el factor francés)
        double saldoProyectado = montoTotalFin;
        if (graTipo.Equals("Total", StringComparison.OrdinalIgnoreCase))
        {
            saldoProyectado = montoTotalFin * Math.Pow(1 + tem, g);
        }

        // Cálculo de cuota fija (método francés con cuota balón)
        double valorPresenteBalon = montoCuotaBalon / Math.Pow(1 + tem, plazoEfectivo);
        double capitalAAmortizar = saldoProyectado - valorPresenteBalon;
        double factorFrances = (tem * Math.Pow(1 + tem, plazoEfectivo)) /
                                (Math.Pow(1 + tem, plazoEfectivo) - 1);
        double cuotaMensualFija = capitalAAmortizar * factorFrances;

        result.MontoTotalFin = montoTotalFin;
        result.MontoCuotaBalon = montoCuotaBalon;
        result.CuotaMensual = cuotaMensualFija;

        // Flujo de caja: posición 0 = desembolso positivo
        var flujoCaja = new List<double>(new double[n + 1]);
        flujoCaja[0] = montoTotalFin;

        double saldoDeudor = montoTotalFin;
        double totalIntereses = 0;
        double totalSeguros = 0;

        for (int mes = 1; mes <= n; mes++)
        {
            double intPeriodo = saldoDeudor * tem;
            double segDesMonto = saldoDeudor * (request.SegDesgrav / 100.0);

            double amoPeriodo;
            double pagoTotal;
            double? interesCapitalizado = null;
            string tipoCuota;

            bool esMesGracia = mes <= g;

            if (esMesGracia)
            {
                if (graTipo.Equals("Total", StringComparison.OrdinalIgnoreCase))
                {
                    // No paga capital ni intereses, el interés se capitaliza
                    amoPeriodo = -intPeriodo;
                    interesCapitalizado = intPeriodo;
                    pagoTotal = segDesMonto + segVehMensual + request.ComPortes;
                    tipoCuota = "Gracia Total";
                }
                else if (graTipo.Equals("Parcial", StringComparison.OrdinalIgnoreCase))
                {
                    // Solo paga intereses
                    amoPeriodo = 0;
                    pagoTotal = intPeriodo + segDesMonto + segVehMensual + request.ComPortes;
                    tipoCuota = "Gracia Parcial";
                }
                else
                {
                    // No debería entrar aquí si gra_meses > 0 implica gracia, pero por seguridad:
                    amoPeriodo = cuotaMensualFija - intPeriodo;
                    pagoTotal = cuotaMensualFija + segDesMonto + segVehMensual + request.ComPortes;
                    tipoCuota = "Regular";
                }
            }
            else
            {
                amoPeriodo = cuotaMensualFija - intPeriodo;
                pagoTotal = cuotaMensualFija + segDesMonto + segVehMensual + request.ComPortes;
                tipoCuota = "Regular";
            }

            double? montoBalonPagado = null;

            // Último mes: se suma la cuota balón
            if (mes == n)
            {
                amoPeriodo += montoCuotaBalon;
                pagoTotal += montoCuotaBalon;
                montoBalonPagado = montoCuotaBalon;
                tipoCuota = "Liquidacion Balon";
            }

            double saldoFinal = saldoDeudor - amoPeriodo;

            result.Cronograma.Add(new CronogramaPagoDto
            {
                Mes = mes,
                TipoCuota = tipoCuota,
                SaldoInicial = Math.Round(saldoDeudor, 2),
                AmoPeriodo = Math.Round(amoPeriodo, 2),
                IntPeriodo = Math.Round(intPeriodo, 2),
                InteresCapitalizado = interesCapitalizado.HasValue ? Math.Round(interesCapitalizado.Value, 2) : null,
                SegDesMonto = Math.Round(segDesMonto, 2),
                SegVehMonto = Math.Round(segVehMensual, 2),
                CuoMensual = Math.Round(cuotaMensualFija, 2),
                MontoBalonPagado = montoBalonPagado.HasValue ? Math.Round(montoBalonPagado.Value, 2) : null,
                PagoTotal = Math.Round(pagoTotal, 2),
                SaldoFinal = Math.Round(saldoFinal, 2)
            });

            flujoCaja[mes] = -pagoTotal;

            totalIntereses += intPeriodo;
            totalSeguros += (segDesMonto + segVehMensual);

            saldoDeudor = saldoFinal;
        }

        result.MonInteresTot = Math.Round(totalIntereses, 2);
        result.MonSeguroTot = Math.Round(totalSeguros, 2);
        result.FlujoCaja = flujoCaja;

        return result;
    }
}