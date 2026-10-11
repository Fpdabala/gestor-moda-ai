using Microsoft.EntityFrameworkCore;
using GestorModaAi.Models;

namespace GestorModaAi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Peca> Pecas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cria o índice na coluna Nome da tabela Pecas para otimizar a busca
            modelBuilder.Entity<Peca>()
                .HasIndex(p => p.Nome);
        }
    }
}