using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class Usuario
{
    [Key]
    [Column("id_funcionario")]
    public int UsuarioId { get; set; }

    [Required]
    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    [Column("email")]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Column("hash_senha")]
    [StringLength(256)]
    public string SenhaHash { get; set; }

    [Required]
    [Column("codigo_pin")]
    [StringLength(4, MinimumLength = 4)]
    public string PinCodigo { get; set; }

    [Required]
    [Column("tag_rfid")]
    [StringLength(50)]
    public string RFIDTag { get; set; }
}

