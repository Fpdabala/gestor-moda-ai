using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorModaAi.Models
{

    [Table("itens_condicional")]
public class ItemCondicional
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("condicional_id")]
    public int CondicionalId { get; set; }

    [Required]
    [Column("peca_id")]
    public int PecaId { get; set; }

    [ForeignKey("PecaId")]
    public Peca? Peca { get; set; }

    [Required]
    [Column("status_item")]
    public string StatusItem { get; set; } = "PENDENTE"; // PENDENTE, COMPRADO, DEVOLVIDO
}

}