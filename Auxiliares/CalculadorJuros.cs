using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SelicBC.Servicos;

namespace SelicBC.Auxiliares
{
    public class ResultadoJuros
    {
        public decimal Principal, JurosMensal, TotalMensal, JurosAnual, TotalAnual;
        public string MesAno = string.Empty;
    }

    public static class CalculadorJuros
    {
        public static async Task<ResultadoJuros?> Calcular()
        {
            Console.Write("Mês/Ano (MM/yyyy): ");
            var ma = Console.ReadLine() ?? "";
            if (!DateTime.TryParseExact(ma, "MM/yyyy", CultureInfo.InvariantCulture,
               DateTimeStyles.None, out _)) { Console.WriteLine("Formato inválido."); return null; }

            Console.Write("Valor inicial (R$): ");
            if (!decimal.TryParse(Console.ReadLine() ?? "", NumberStyles.Number,
               CultureInfo.InvariantCulture, out var p)) { Console.WriteLine("Valor inválido."); return null; }

            var rec = (await new ServicoSelic().ObterAsync(DateTime.ParseExact(ma, "MM/yyyy",
              CultureInfo.InvariantCulture), DateTime.ParseExact(ma, "MM/yyyy",
              CultureInfo.InvariantCulture))).First();
            double ad = (double)(rec.Valor / 100m);
            double md = Math.Pow(1 + ad, 1.0 / 12.0) - 1;

            var raw = p * (decimal)(1 + md);
            var jm = Math.Round(raw - p, 2, MidpointRounding.AwayFromZero);
            var tm = p + jm;
            var ja = Math.Round(p * (decimal)ad, 2, MidpointRounding.AwayFromZero);
            var ta = p + ja;

            return new ResultadoJuros
            {
                Principal = p,
                MesAno = ma,
                JurosMensal = jm,
                TotalMensal = tm,
                JurosAnual = ja,
                TotalAnual = ta
            };
        }
    }
}
