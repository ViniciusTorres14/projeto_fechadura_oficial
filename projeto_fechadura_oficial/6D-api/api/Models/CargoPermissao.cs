using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class CargoPermissao
{
    [Key]
    [Column("id_permissao_cargo")]
    public int CargoPermissaoId { get; set; }

    [Required]
    [Column("id_cargo")]
    public int CargoId { get; set; }

    [Required]
    [Column("id_permissao")]
    public int PermissaoId { get; set; }

    [ForeignKey("CargoId")]
    public Cargo Cargo { get; set; }

    [ForeignKey("PermissaoId")]
    public Permissao Permissao { get; set; }
}

