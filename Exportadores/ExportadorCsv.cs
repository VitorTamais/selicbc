using System.Collections.Generic;
using System.IO;
using System.Text;
using SelicBC.Core;
using SelicBC.Modelos;
using SelicBC.Auxiliares;

namespace SelicBC.Exportadores
{
    public class ExportadorCsv : IExportador<RegistroSelic>
    {
        public void Exportar(IEnumerable<RegistroSelic> dados, string caminho)
        {
            using var sw = new StreamWriter(caminho, false, new UTF8Encoding(true));
            sw.WriteLine("Data - Anual(%) - MensalSimples(%)");
            foreach (var r in dados)
            {
                var ms = ConversorTaxa.ParaMensalSimples(r.Valor);
                sw.WriteLine($"{r.Data:dd/MM/yyyy} - {r.Valor:F2} - {ms:F2}");
            }
        }
    }
}
