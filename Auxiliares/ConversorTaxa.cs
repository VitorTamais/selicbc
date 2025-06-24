using System;
namespace SelicBC.Auxiliares
{
    public static class ConversorTaxa
    {
        public static decimal ParaMensalSimples(decimal pct) => pct / 12m;
        public static decimal ParaMensalComposta(decimal pct)
        {
            double a = (double)(pct / 100m);
            double m = Math.Pow(1 + a, 1 / 12.0) - 1;
            return (decimal) m * 100m;
        }
    }
}
