using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class UsuarioSalaAcesso
{
    [Key]
    [Column("id_acesso")]
    public int AcessoId { get; set; }

    [Column("id_funcionario")]
    public int? UsuarioId { get; set; }

    [Column("id_sala")]
    public int? SalaId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }

    [ForeignKey("SalaId")]
    public Sala? Sala { get; set; }
}

