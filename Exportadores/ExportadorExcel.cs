using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SelicBC.Core;
using SelicBC.Modelos;
using SelicBC.Auxiliares;

namespace SelicBC.Exportadores
{
    public class ExportadorExcel : IExportador<RegistroSelic>
    {
        public void Exportar(IEnumerable<RegistroSelic> dados, string caminho)
        {
            ExcelPackage.License.SetNonCommercialPersonal("SelicBCUser");
            using var pkg = new ExcelPackage(new FileInfo(caminho));
            var ws = pkg.Workbook.Worksheets.Add("Selic");

            ws.Cells["A1:C1"].Merge = true;
            ws.Cells[1, 1].Value = "Relatório de Taxa Selic";
            ws.Cells[1, 1].Style.Font.Size = 16;
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells[2, 1].Value = "Data";
            ws.Cells[2, 2].Value = "Taxa Anual (%)";
            ws.Cells[2, 3].Value = "Taxa Mensal Simples (%)";
            using (var range = ws.Cells["A2:C2"])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            int linha = 3;
            foreach (var rec in dados)
            {
                ws.Cells[linha, 1].Value = rec.Data;
                ws.Cells[linha, 1].Style.Numberformat.Format = "dd/MM/yyyy";
                ws.Cells[linha, 2].Value = rec.Valor;
                ws.Cells[linha, 2].Style.Numberformat.Format = "0.00";
                var ms = ConversorTaxa.ParaMensalSimples(rec.Valor);
                ws.Cells[linha, 3].Value = ms;
                ws.Cells[linha, 3].Style.Numberformat.Format = "0.00";
                ws.Cells[linha, 1, linha, 3].Style.Border.BorderAround(ExcelBorderStyle.Dotted);
                linha++;
            }

            ws.Column(1).Width = 15;
            ws.Column(2).Width = 18;
            ws.Column(3).Width = 22;

            ws.Cells[$"A{linha + 1}"].Value = $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Cells[linha + 1, 1, linha + 1, 3].Merge = true;
            ws.Cells[linha + 1, 1].Style.Font.Italic = true;
            ws.Cells[linha + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            pkg.Save();
        }
    }
}
