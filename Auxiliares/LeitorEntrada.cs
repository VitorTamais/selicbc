using System;
using System.Globalization;

namespace SelicBC.Auxiliares
{
    public static class LeitorEntrada
    {
        public static (DateTime, DateTime) LerIntervalo()
        {
            DateTime s, e;
            Console.Write("Data inicial (dd/MM/yyyy): ");
            while (!DateTime.TryParseExact(Console.ReadLine() ?? "", "dd/MM/yyyy",
                  CultureInfo.InvariantCulture, DateTimeStyles.None, out s))
                Console.Write("Formato inválido. Tente novamente: ");
            Console.Write("Data final   (dd/MM/yyyy): ");
            while (!DateTime.TryParseExact(Console.ReadLine() ?? "", "dd/MM/yyyy",
                  CultureInfo.InvariantCulture, DateTimeStyles.None, out e))
                Console.Write("Formato inválido. Tente novamente: ");
            return (s, e);
        }
    }
}
