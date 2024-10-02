namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosDAO _usuariosDao;

        public UsuariosController(UsuariosDAO usuariosDao)
        {
            _usuariosDao = usuariosDao;
        }

        [HttpGet]
        public IActionResult GetAllEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var usuarios = _usuariosDao.ReadAll(pageNumber, pageSize);
            var totalCount = _usuariosDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, Usuarios = usuarios });
        }

        [HttpGet("{usuarioId:int}")]
        public IActionResult GetEmployeeById(int usuarioId)
        {
            var usuario = _usuariosDao.ReadById(usuarioId);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] CreateUsuarioDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = new Usuario
            {
                Nome = createDto.Nome,
                Email = createDto.Email,
                PinCodigo = _usuariosDao.GenerateUniquePinCode(),
                RFIDTag = !string.IsNullOrEmpty(createDto.RFIDTag) ? _usuariosDao.GenerateUniqueRFIDTag() : null,
                SenhaHash = _usuariosDao.HashPassword(createDto.Password) // Assuming password is provided
            };

            _usuariosDao.Create(usuario);

            var usuarioDto = new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                PinCodigo = usuario.PinCodigo,
                RFIDTag = usuario.RFIDTag
            };

            return CreatedAtAction(nameof(GetEmployeeById), new { usuarioId = usuario.UsuarioId }, usuarioDto);
        }

        [HttpPut("{usuarioId:int}")]
        public IActionResult UpdateEmployee(int usuarioId, [FromBody] UpdateUsuarioDto updateDto)
        {
            if (usuarioId != updateDto.UsuarioId) return BadRequest("ID mismatch.");
            if (_usuariosDao.ReadById(usuarioId) == null) return NotFound();

            var usuario = new Usuario
            {
                UsuarioId = updateDto.UsuarioId,
                Nome = updateDto.Nome,
                Email = updateDto.Email,
                PinCodigo = !string.IsNullOrEmpty(updateDto.PinCodigo) ? updateDto.PinCodigo : _usuariosDao.GenerateUniquePinCode(),
                RFIDTag = !string.IsNullOrEmpty(updateDto.RFIDTag) ? updateDto.RFIDTag : null,
                // Assuming password update is handled separately
            };

            _usuariosDao.Update(usuario);
            return Ok(usuario);
        }

        [HttpDelete("{usuarioId:int}")]
        public IActionResult DeleteEmployee(int usuarioId)
        {
            if (_usuariosDao.ReadById(usuarioId) == null) return NotFound();

            _usuariosDao.Delete(usuarioId);
            return NoContent();
        }
    }

    // DTOs
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string? RFIDTag { get; set; }
        public string PinCodigo { get; set; }
        public string Email { get; set; }
    }

    public class CreateUsuarioDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(10)]
        public string? PinCodigo { get; set; }

        public string? RFIDTag { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Password { get; set; } // Added Password field
    }

    public class UpdateUsuarioDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(4, MinimumLength = 4)]
        public string? PinCodigo { get; set; }

        public string? RFIDTag { get; set; }

        // Optionally, you can add fields for password updates
    }
}
