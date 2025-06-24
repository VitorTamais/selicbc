using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SelicBC.Core;
using SelicBC.Auxiliares.Conversores;
using SelicBC.Modelos;

namespace SelicBC.Servicos
{
    public class ServicoSelic : IObterDados<RegistroSelic>
    {
        private static readonly HttpClient http = new();
        private const int MaxAnos = 10;

        public async Task<IEnumerable<RegistroSelic>> ObterAsync(
            DateTime inicio, DateTime fim)
        {
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            opts.Converters.Add(new ConversorData());
            opts.Converters.Add(new ConversorDecimal());

            var lista = new List<RegistroSelic>();
            foreach (var (s, f) in BuildPeriods(inicio, fim))
            {
                var url = $"https://api.bcb.gov.br/dados/serie/" +
                          $"bcdata.sgs.4390/dados?formato=json" +
                          $"&dataInicial={s:dd/MM/yyyy}&dataFinal={f:dd/MM/yyyy}";
                var json = await http.GetStringAsync(url);
                var regs = JsonSerializer.Deserialize<List<RegistroSelic>>(json, opts);
                if (regs != null) lista.AddRange(regs);
            }
            return lista.OrderBy(x => x.Data);
        }

        private static IEnumerable<(DateTime, DateTime)> BuildPeriods(
            DateTime i, DateTime f)
        {
            var periods = new List<(DateTime, DateTime)>();
            var cur = i;
            while ((f - cur).TotalDays > MaxAnos * 365)
            {
                var mid = cur.AddYears(MaxAnos);
                periods.Add((cur, mid));
                cur = mid.AddDays(1);
            }
            periods.Add((cur, f));
            return periods;
        }
    }
}
