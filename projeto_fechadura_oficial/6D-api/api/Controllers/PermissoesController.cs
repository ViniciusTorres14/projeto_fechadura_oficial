namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissoesController : ControllerBase
    {
        private readonly PermissoesDAO _permissoesDao;

        public PermissoesController(PermissoesDAO permissoesDao)
        {
            _permissoesDao = permissoesDao;
        }

        [HttpGet]
        public IActionResult GetAllPermissions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var permissoes = _permissoesDao.ReadAll(pageNumber, pageSize);
            var totalCount = _permissoesDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, Permissoes = permissoes });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPermissionById(int id)
        {
            var permissao = _permissoesDao.ReadById(id);
            if (permissao == null) return NotFound();
            return Ok(permissao);
        }

        [HttpPost]
        public IActionResult CreatePermission([FromBody] CreatePermissaoDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var permissao = new Permissao
            {
                PermissionKey = createDto.PermissionKey,
                Descricao = createDto.Descricao
            };

            _permissoesDao.Create(permissao);

            var permissaoDto = new PermissaoDto
            {
                PermissaoId = permissao.PermissaoId,
                PermissionKey = permissao.PermissionKey,
                Descricao = permissao.Descricao
            };

            return CreatedAtAction(nameof(GetPermissionById), new { id = permissao.PermissaoId }, permissaoDto);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePermission(int id, [FromBody] UpdatePermissaoDto updateDto)
        {
            if (id != updateDto.PermissaoId) return BadRequest("ID mismatch.");
            if (_permissoesDao.ReadById(id) == null) return NotFound();

            var permissao = new Permissao
            {
                PermissaoId = updateDto.PermissaoId,
                PermissionKey = updateDto.PermissionKey,
                Descricao = updateDto.Descricao
            };

            _permissoesDao.Update(permissao);
            return Ok(permissao);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePermission(int id)
        {
            if (_permissoesDao.ReadById(id) == null) return NotFound();

            _permissoesDao.Delete(id);
            return NoContent();
        }
    }

    // DTOs
    public class PermissaoDto
    {
        public int PermissaoId { get; set; }
        public string PermissionKey { get; set; }
        public string? Descricao { get; set; }
    }

    public class CreatePermissaoDto
    {
        [Required]
        [StringLength(50)]
        public string PermissionKey { get; set; }

        public string? Descricao { get; set; }
    }

    public class UpdatePermissaoDto
    {
        [Required]
        public int PermissaoId { get; set; }

        [Required]
        [StringLength(50)]
        public string PermissionKey { get; set; }

        public string? Descricao { get; set; }
    }
}
