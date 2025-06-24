using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SelicBC.Dados;
using SelicBC.Exportadores;
using SelicBC.Auxiliares;
using SelicBC.Modelos;
using SelicBC.Servicos;

class Program
{
    static async Task Main()
    {
        var logs = new List<LogAcao>();
        var serv = new ServicoSelic();
        var down = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.UserProfile), "Downloads");
        using var db = new ContextoAplicacao();
        db.Database.Migrate();
        bool sair = false;

        while (!sair)
        {
            Console.Clear();
            Console.WriteLine("===== SelicBC =====\n");
            Console.WriteLine("1) Coletar Dados API");
            Console.WriteLine("2) Exibir Dados (2.1 mais recente, 2.2 mais antiga)");
            Console.WriteLine("3) Exportar Dados (CSV & EXCEL)");
            Console.WriteLine("4) Calcular Juros Compostos");
            Console.WriteLine("5) Fazer Backup");
            Console.WriteLine("6) Visualizar Backups");
            Console.WriteLine("9) Sair\n");
            Console.Write("Escolha uma opção: ");
            var op = Console.ReadLine() ?? "";
            var log = new LogAcao { DataHora = DateTime.Now, Opcao = op };

            switch (op)
            {
                case "1":
                    var d0 = new DateTime(1986, 8, 1); var d1 = DateTime.Today;
                    var r0 = (await serv.ObterAsync(d0, d1)).ToList();
                    log.Entrada = $"{d0:dd/MM/yyyy} a {d1:dd/MM/yyyy}";
                    log.Retorno = $"Total registros: {r0.Count}";
                    Console.WriteLine($"\n{log.Retorno}\n");
                    break;
                case "2":
                case "2.1":
                case "2.2":
                    var (s, e) = LeitorEntrada.LerIntervalo();
                    var r1 = (await serv.ObterAsync(s, e)).ToList();
                    if (op == "2.2") r1.Reverse();
                    log.Entrada = $"{s:dd/MM/yyyy} a {e:dd/MM/yyyy}";
                    log.Retorno = $"Exibidos: {r1.Count}";
                    Console.WriteLine("\nData       | Anual(%) | MensalSimples(%)");
                    r1.ForEach(r => Console.WriteLine(
                      $"{r.Data:dd/MM/yyyy} | {r.Valor:F2}     | {ConversorTaxa.ParaMensalSimples(r.Valor):F2}"));
                    Console.WriteLine();
                    break;
                case "3":
                    var (i2, f2) = LeitorEntrada.LerIntervalo();
                    var rd = (await serv.ObterAsync(i2, f2)).ToList();
                    log.Entrada = $"{i2:dd/MM/yyyy} a {f2:dd/MM/yyyy}";
                    Console.Write("\n1)CSV");
                    Console.Write("\n2)EXCEL");
                    Console.Write("\nEscolha uma opção: ");
                    var fm = Console.ReadLine() ?? "";
                    if (fm == "1")
                    {
                        var p = Path.Combine(down, $"selic_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                        new ExportadorCsv().Exportar(rd, p);
                        log.Retorno = $"CSV salvo em: {p}";
                    }
                    else
                    {
                        var p = Path.Combine(down, $"selic_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                        new ExportadorExcel().Exportar(rd, p);
                        log.Retorno = $"Excel salvo em: {p}";
                    }
                    Console.WriteLine($"\n{log.Retorno}\n");
                    break;
                case "4":
                    var res = await CalculadorJuros.Calcular();
                    if (res != null)
                    {
                        log.Entrada = $"Mês/Ano:{res.MesAno};Valor:{res.Principal:C2}";
                        log.Retorno = $"JM:{res.JurosMensal:C2}; TM:{res.TotalMensal:C2}; " +
                                   $"JA:{res.JurosAnual:C2}; TA:{res.TotalAnual:C2}";
                        Console.WriteLine($"\nOriginal     : {res.Principal:C2}");
                        Console.WriteLine($"Juros Mensal : {res.JurosMensal:C2}");
                        Console.WriteLine($"Total Mensal : {res.TotalMensal:C2}");
                        Console.WriteLine($"Juros Anual  : {res.JurosAnual:C2}");
                        Console.WriteLine($"Total Anual  : {res.TotalAnual:C2}\n");
                    }
                    break;
                case "5":
                    var sv = logs.Where(l => l.Entrada != "" || l.Retorno != "").ToList();
                    if (sv.Count > 0)
                    {
                        db.Logs.AddRange(sv);
                        await db.SaveChangesAsync();
                        logs.Clear();
                        Console.WriteLine("\nBackup salvo com sucesso!\n");
                    }
                    else Console.WriteLine("\nNada para salvar.\n");
                    break;
                case "6":
                    var all = await db.Logs.OrderBy(l => l.DataHora).ToListAsync();
                    all.ForEach(l => {
                        Console.WriteLine($"\nDATA/HORA  : {l.DataHora:dd/MM/yyyy HH:mm}");
                        Console.WriteLine(new string('-', 40));
                        Console.WriteLine($"OPÇÃO      : {l.Opcao}");
                        Console.WriteLine($"ENTRADA    : {l.Entrada}");
                        Console.WriteLine($"RETORNO    : {l.Retorno}");
                        Console.WriteLine(new string('=', 40));
                    });
                    Console.WriteLine();
                    break;
                case "9":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("\nOpção inválida!\n");
                    break;
            }
            if (log.Entrada != "" || log.Retorno != "") logs.Add(log);
            if (!sair)
            {
                Console.WriteLine("ENTER para continuar..."); Console.ReadLine();
            }
        }
    }
}
