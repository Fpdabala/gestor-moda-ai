using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorModaAi.Data;
using GestorModaAi.Models;

namespace GestorModaAi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> Listar()
    {
        return await _context.Categorias.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> Criar(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Listar), new { id = categoria.Id }, categoria);
    }
}
