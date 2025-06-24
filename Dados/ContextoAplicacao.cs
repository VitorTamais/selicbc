using Microsoft.EntityFrameworkCore;
using SelicBC.Modelos;
namespace SelicBC.Dados
{
    public class ContextoAplicacao : DbContext
    {
        public DbSet<RegistroSelic> Registros { get; set; }
        public DbSet<LogAcao> Logs { get; set; }
        private string CaminhoDB { get; }
        public ContextoAplicacao()
        {
            var pasta = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            CaminhoDB = Path.Combine(pasta, "selicbc.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder opc)
            => opc.UseSqlite($"Data Source={CaminhoDB}");
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<RegistroSelic>().ToTable("Registros");
            mb.Entity<LogAcao>().ToTable("Logs");
        }
    }
}
