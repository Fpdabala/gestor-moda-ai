using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorModaAi.Models;

[Table("pecas")]
public class Peca
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("categoria_id")]
    public int CategoriaId { get; set; }

    // Propriedade de navegação para o Entity Framework conseguir fazer o Include se necessário
    [ForeignKey("CategoriaId")]
    public Categoria? CategoriaObj { get; set; }

    [Required(ErrorMessage = "O nome da peça é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    // Propriedade para satisfazer o código antigo que pedia string Categoria nos controllers
    [NotMapped]
    public string? Categoria { get; set; }

    [Column("tamanho")]
    public string? Tamanho { get; set; }

    [Range(0.01, 10000.00, ErrorMessage = "O preço de custo deve ser maior que zero.")]
    [Column("preco_custo")]
    public decimal PrecoCusto { get; set; }

    [Range(0.01, 10000.00, ErrorMessage = "O preço de venda deve ser maior que zero.")]
    [Column("preco_venda")]
    public decimal PrecoVenda { get; set; }

    // Propriedade temporária para evitar o erro de compilação no controller antigo
    [NotMapped]
    public int Estoque { get; set; } = 10;

    [Column("status")]
    public string Status { get; set; } = "DISPONIVEL";

    [Column("ativo")]
    public bool Ativo { get; set; } = true;
}