using System.ComponentModel.DataAnnotations;

namespace GestorModaAi.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    public string Cpf { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string? Email { get; set; }

    public string? Telefone { get; set; }

    // Decisão de Arquitetura exigida pela banca: Exclusão Lógica
    public bool Ativo { get; set; } = true;
}