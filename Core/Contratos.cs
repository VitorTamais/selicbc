namespace SelicBC.Core
{
    public interface IObterDados<T>
    {
        Task<IEnumerable<T>> ObterAsync(DateTime inicio, DateTime fim);
    }
    public interface IExportador<T>
    {
        void Exportar(IEnumerable<T> dados, string caminho);
    }
}
