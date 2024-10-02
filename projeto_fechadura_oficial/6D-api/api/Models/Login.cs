using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_webapi.Models;
public class LoginModel
{
    [Column("nome_usuario")]
    public string? Username { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Required]
    [Column("senha")]
    public string Password { get; set; }
}
