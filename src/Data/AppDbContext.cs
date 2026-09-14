using Microsoft.EntityFrameworkCore;
using GestorModaAi.Models;

namespace GestorModaAi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();
}
