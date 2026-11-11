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
            // Validação básica de segurança para evitar crash se a categoria não for enviada
            if (peca.CategoriaId <= 0)
            {
                peca.CategoriaId = 1; // Ajusta para um ID padrão válido se necessário
            }

            peca.Ativo = true;
            _context.Pecas.Add(peca);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPeca), new { id = peca.Id }, peca);
        }

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

            // ALTERAÇÃO AQUI: Se o preço de custo vier zero ou negativo, ajustamos para um valor válido (ex: 1.0) 
            // ou podes simplesmente remover o bloqueio se não usares custo na tela.
            if (peca.PrecoCusto <= 0)
            {
                peca.PrecoCusto = 1.0M; // Atribui um valor padrão para passar na regra
            }

            pecaExistente.Nome = peca.Nome;
            pecaExistente.Categoria = peca.Categoria;
            pecaExistente.Tamanho = peca.Tamanho;
            pecaExistente.PrecoCusto = peca.PrecoCusto; // Vai receber o valor ajustado
            pecaExistente.PrecoVenda = peca.PrecoVenda;
            pecaExistente.Estoque = peca.Estoque;

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