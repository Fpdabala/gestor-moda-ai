using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorModaAi.Data;
using GestorModaAi.Models;

namespace GestorModaAi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.Where(c => c.Ativo).ToListAsync();
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null || !cliente.Ativo)
            {
                return NotFound(new { mensagem = "Cliente não encontrado." });
            }

            return cliente;
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            var existeCliente = await _context.Clientes
                .AnyAsync(c => c.Cpf == cliente.Cpf || c.Email == cliente.Email);

            if (existeCliente)
            {
                return BadRequest(new { mensagem = "Já existe um cliente cadastrado com este CPF ou E-mail." });
            }

            cliente.Ativo = true;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(new { mensagem = "O ID da URL não confere com o ID do objeto." });
            }

            var clienteExistente = await _context.Clientes.FindAsync(id);
            if (clienteExistente == null || !clienteExistente.Ativo)
            {
                return NotFound(new { mensagem = "Cliente não encontrado." });
            }

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Telefone = cliente.Telefone;

            _context.Entry(clienteExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.Id == id))
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

        // DELETE: api/clientes/5 (EXCLUSÃO LÓGICA)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null || !cliente.Ativo)
            {
                return NotFound(new { mensagem = "Cliente não encontrado." });
            }

            cliente.Ativo = false;
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Cliente desativado com sucesso (exclusão lógica aplicada para preservar histórico)." });
        }
    }
}