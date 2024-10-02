using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class Sala
{
    [Key]
    [Column("id_sala")]
    public int SalaId { get; set; }

    [Required]
    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; }

    [Column("descricao")]
    public string? Descricao { get; set; }

    [Required]
    [Column("status")]
    public bool Status { get; set; } = false;

    [Column("imagem", TypeName = "LONGBLOB")]
    public byte[]? Imagem { get; set; }

    [Column("ocupado_por_funcionario_id")]
    public int? OcupadoPorUsuarioId { get; set; }

    [ForeignKey("OcupadoPorUsuarioId")]
    public Usuario? OcupadoPorUsuario { get; set; }
}

