using System;
namespace SelicBC.Modelos
{
    public class LogAcao
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Opcao { get; set; } = string.Empty;
        public string Entrada { get; set; } = string.Empty;
        public string Retorno { get; set; } = string.Empty;
    }
}
