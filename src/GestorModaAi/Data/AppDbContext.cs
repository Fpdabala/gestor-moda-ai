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
        public DbSet<Condicional> Condicionais { get; set; }
        public DbSet<ItemCondicional> ItensCondicional { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cria o índice na coluna Nome da tabela Pecas
            modelBuilder.Entity<Peca>()
                .HasIndex(p => p.Nome);

            // Mapeamento explícito exato para as tabelas no PostgreSQL
            modelBuilder.Entity<Cliente>().ToTable("clientes");
            modelBuilder.Entity<Categoria>().ToTable("categorias");
            modelBuilder.Entity<Peca>().ToTable("pecas");
            modelBuilder.Entity<Condicional>().ToTable("condicionais");
            modelBuilder.Entity<ItemCondicional>().ToTable("itens_condicional");
        }

    }
}