using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorModaAi.Models
{
    [Table("condicionais")]
    public class Condicional
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("cliente_id")]
        public int ClienteId { get; set; }

        // Propriedade de navegação para o Cliente (opcional, para incluir dados no JSON)
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        [Required]
        [Column("data_saida")]
        public DateTime DataSaida { get; set; } = DateTime.Now;

        [Required]
        [Column("data_limite_devolucao")]
        public DateTime DataLimiteDevolucao { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; } = "EM_ANDAMENTO"; // EM_ANDAMENTO, FINALIZADA, ATRASADA

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        // Lista de itens da condicional
        public ICollection<ItemCondicional> Itens { get; set; } = new List<ItemCondicional>();
    }
}

