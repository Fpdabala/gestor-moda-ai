using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorModaAi.Models;

[Table("clientes")]
public class Cliente
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("whatsapp")]
    public string Whatsapp { get; set; } = string.Empty;

    [Column("tamanho_preferido")]
    public string? TamanhoPreferido { get; set; }

    [Column("estilo")]
    public string? Estilo { get; set; }

    [Column("ativo")]
    public bool Ativo { get; set; } = true;
    
    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Propriedades temporárias para satisfazer o ClientesController que tenta ler estes campos
    [NotMapped]
    public string? Cpf { get; set; }

    [NotMapped]
    public string? Email { get; set; }

    [NotMapped]
    public string? Telefone { get; set; }
}