using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorModaAi.Data;

namespace GestorModaAi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/dashboard/pecas-por-categoria
        [HttpGet("pecas-por-categoria")]
        public async Task<IActionResult> GetPecasPorCategoria()
        {
            // Equivalente a um GROUP BY em SQL: agrupa as peças pelo nome da categoria e conta a quantidade
            var resultado = await _context.Pecas
                .Where(p => p.Ativo)
                .GroupBy(p => p.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    TotalPecas = g.Count()
                })
                .ToListAsync();

            return Ok(resultado);
        }

        // GET: api/dashboard/resumo-geral
        [HttpGet("resumo-geral")]
        public async Task<IActionResult> GetResumoGeral()
        {
            var totalClientes = await _context.Clientes.CountAsync(c => c.Ativo);
            var totalPecas = await _context.Pecas.CountAsync(p => p.Ativo);

            var resumo = new
            {
                TotalClientesAtivos = totalClientes,
                TotalPecasAtivas = totalPecas
            };

            return Ok(resumo);
        }
    }
}