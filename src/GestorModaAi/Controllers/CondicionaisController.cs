using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorModaAi.Data;
using GestorModaAi.Models;

namespace GestorModaAi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CondicionaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CondicionaisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/condicionais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Condicional>>> GetCondicionais()
        {
            // Vamos testar apenas a tabela principal para ver se o erro 42P01 some
            return await _context.Condicionais.ToListAsync();
        }

        // POST: api/condicionais (Criar nova condicional e atualizar status das peças)
        [HttpPost]
        public async Task<ActionResult<Condicional>> PostCondicional(Condicional condicional)
        {
            _context.Condicionais.Add(condicional);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCondicionais), new { id = condicional.Id }, condicional);
        }

        // PUT: api/condicionais/5/finalizar (Finaliza a condicional e devolve/baixa stock)
        [HttpPut("{id}/finalizar")]
        public async Task<IActionResult> FinalizarCondicional(int id)
        {
            var condicional = await _context.Condicionais
                .Include(c => c.Itens)
                .ThenInclude(i => i.Peca)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (condicional == null)
            {
                return NotFound(new { mensagem = "Condicional não encontrada." });
            }

            condicional.Status = "FINALIZADA";

            // Atualiza os itens e o estoque das peças se necessário
            foreach (var item in condicional.Itens)
            {
                item.StatusItem = "COMPRADO";
                if (item.Peca != null)
                {
                    // Se a peça foi vendida, desconta do estoque ou ajusta conforme regra do negócio
                    // item.Peca.Estoque -= 1; 
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}