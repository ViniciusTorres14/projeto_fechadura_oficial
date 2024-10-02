using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class Permissao
{
    [Key]
    [Column("id_permissao")]
    public int PermissaoId { get; set; }
    
    [Required]
    [Column("chave_permissao")]
    [StringLength(50)]
    public string PermissionKey { get; set; }
    
    [Column("descricao")]
    public string? Descricao { get; set; }
}

