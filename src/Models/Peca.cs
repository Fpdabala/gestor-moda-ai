using System.ComponentModel.DataAnnotations;

namespace GestorModaAi.Models;

public class Peca
{
	public int Id { get; set; }

	[Required(ErrorMessage = "O nome da peça é obrigatório.")]
	[StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
	public string Nome { get; set; } = string.Empty;

	public string? Categoria { get; set; }

	[Range(0.01, 10000.00, ErrorMessage = "O preço de custo deve ser maior que zero.")]
	public decimal PrecoCusto { get; set; }

	[Range(0.01, 10000.00, ErrorMessage = "O preço de venda deve ser maior que zero.")]
	public decimal PrecoVenda { get; set; }

	[Range(0, 10000, ErrorMessage = "O estoque não pode ser negativo.")]
	public int Estoque { get; set; }

	// Decisão de Arquitetura exigida pela banca: Exclusão Lógica
	public bool Ativo { get; set; } = true;
}