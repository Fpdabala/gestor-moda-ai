using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorModaAi.Data;
using GestorModaAi.Models;

namespace GestorModaAi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PecasController : ControllerBase
	{
		private readonly AppDbContext _context;

		public PecasController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/pecas
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Peca>>> GetPecas()
		{
			return await _context.Pecas.Where(p => p.Ativo).ToListAsync();
		}

		// GET: api/pecas/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Peca>> GetPeca(int id)
		{
			var peca = await _context.Pecas.FindAsync(id);

			if (peca == null || !peca.Ativo)
			{
				return NotFound(new { mensagem = "Peça não encontrada." });
			}

			return peca;
		}

		// POST: api/pecas
		[HttpPost]
		public async Task<ActionResult<Peca>> PostPeca(Peca peca)
		{
			// Validação de preço de custo
			if (peca.PrecoCusto < 0)
			{
				return BadRequest(new { mensagem = "O preço de custo não pode ser negativo." });
			}

			// Tratamento de duplicidade por nome da peça
			var existePeca = await _context.Pecas
				.AnyAsync(p => p.Nome.ToLower() == peca.Nome.ToLower() && p.Ativo);

			if (existePeca)
			{
				return BadRequest(new { mensagem = "Já existe uma peça ativa cadastrada com este nome." });
			}

			peca.Ativo = true;
			_context.Pecas.Add(peca);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetPeca), new { id = peca.Id }, peca);
		}

		// PUT: api/pecas/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutPeca(int id, Peca peca)
		{
			if (id != peca.Id)
			{
				return BadRequest(new { mensagem = "O ID da URL não confere com o ID do objeto." });
			}

			var pecaExistente = await _context.Pecas.FindAsync(id);
			if (pecaExistente == null || !pecaExistente.Ativo)
			{
				return NotFound(new { mensagem = "Peça não encontrada." });
			}

			if (peca.PrecoCusto < 0)
			{
				return BadRequest(new { mensagem = "O preço de custo não pode ser negativo." });
			}

			pecaExistente.Nome = peca.Nome;
			pecaExistente.Categoria = peca.Categoria;
			pecaExistente.PrecoCusto = peca.PrecoCusto;

			_context.Entry(pecaExistente).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!_context.Pecas.Any(e => e.Id == id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// DELETE: api/pecas/5 (EXCLUSÃO LÓGICA)
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePeca(int id)
		{
			var peca = await _context.Pecas.FindAsync(id);
			if (peca == null || !peca.Ativo)
			{
				return NotFound(new { mensagem = "Peça não encontrada." });
			}

			// Exclusão lógica para preservar histórico de vendas e sacolas condicionais
			peca.Ativo = false;
			_context.Entry(peca).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return Ok(new { mensagem = "Peça desativada com sucesso (exclusão lógica aplicada)." });
	
		}

        // GET: api/pecas/buscar?termo=vestido
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Peca>>> BuscarPecas([FromQuery] string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                return BadRequest(new { mensagem = "O termo de busca não pode ser vazio." });
            }

            // Busca por texto ignorando maiúsculas/minúsculas e filtrando apenas as ativas
            var pecas = await _context.Pecas
                .Where(p => p.Ativo && (p.Nome.Contains(termo) || p.Categoria.Contains(termo)))
                .ToListAsync();

            return Ok(pecas);
        }

	 }

}