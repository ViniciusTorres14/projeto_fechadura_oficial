using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class RegistrosDeAcesso
{
    [Key]
    [Column("id_registro")]
    public int RegistroId { get; set; }

    [Column("id_funcionario")]
    public int? UsuarioId { get; set; }

    [Column("id_sala")]
    public int? SalaId { get; set; }

    [Column("tempo_acesso")]
    public DateTime? AccessTime { get; set; } = DateTime.UtcNow;

    [Column("acesso_concedido")]
    public bool? AccessGranted { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }

    [ForeignKey("SalaId")]
    public Sala? Sala { get; set; }
}
