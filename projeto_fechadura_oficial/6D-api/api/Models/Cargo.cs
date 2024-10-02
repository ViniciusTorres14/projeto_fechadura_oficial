using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class Cargo
{
    [Key]
    [Column("id_cargo")]
    public int CargoId { get; set; }

    [Required]
    [Column("nome_cargo")]
    [StringLength(50)]
    public string RoleName { get; set; }

    [Column("descricao")]
    public string? Descricao { get; set; }

    public ICollection<CargoPermissao> CargoPermissoes { get; set; }
}

