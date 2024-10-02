using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _6D.Models;

public class TokenUsuario
{
    [Key]
    [Column("id_token")]
    public int TokenId { get; set; }

    [Column("id_funcionario")]
    public int UsuarioId { get; set; }

    [Column("token")]
    [MaxLength(1024)]
    public string Token { get; set; }

    [Column("expiracao")]
    public DateTime Expiration { get; set; }

    [Column("criado_em")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }
}

