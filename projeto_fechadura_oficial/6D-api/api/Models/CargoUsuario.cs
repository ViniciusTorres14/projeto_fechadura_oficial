using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class UsuarioCargo
{
    [Key]
    [Column("id_funcionario_cargo")]
    public int EmployeeRoleId { get; set; }

    [Column("id_funcionario")]
    public int? UsuarioId { get; set; }

    [Column("id_cargo")]
    public int? CargoId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }

    [ForeignKey("CargoId")]
    public Cargo? Cargo { get; set; }
}

