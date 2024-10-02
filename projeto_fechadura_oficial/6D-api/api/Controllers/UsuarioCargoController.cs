namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioCargoController : ControllerBase
    {
        private readonly UsuarioCargoDAO _usuarioCargoDao;

        public UsuarioCargoController(UsuarioCargoDAO usuarioCargoDao)
        {
            _usuarioCargoDao = usuarioCargoDao;
        }

        [HttpGet]
        public IActionResult GetAllEmployeeRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var usuarioCargos = _usuarioCargoDao.ReadAll(pageNumber, pageSize);
            var totalCount = _usuarioCargoDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, UsuarioCargos = usuarioCargos });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployeeRoleById(int id)
        {
            var usuarioCargo = _usuarioCargoDao.ReadById(id);
            if (usuarioCargo == null) return NotFound();
            return Ok(usuarioCargo);
        }

        [HttpPost]
        public IActionResult CreateEmployeeRole([FromBody] UsuarioCargoDto usuarioCargoDto)
        {
            if (usuarioCargoDto == null) return BadRequest();

            var usuarioCargo = new UsuarioCargo
            {
                UsuarioId = usuarioCargoDto.UsuarioId,
                CargoId = usuarioCargoDto.CargoId
            };

            _usuarioCargoDao.Create(usuarioCargo);
            return CreatedAtAction(nameof(GetEmployeeRoleById), new { id = usuarioCargo.EmployeeRoleId }, usuarioCargo);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateEmployeeRole(int id, [FromBody] UsuarioCargoDto usuarioCargoDto)
        {
            if (id != usuarioCargoDto.EmployeeRoleId) return BadRequest("ID mismatch.");
            var existingRole = _usuarioCargoDao.ReadById(id);
            if (existingRole == null) return NotFound();

            var usuarioCargo = new UsuarioCargo
            {
                EmployeeRoleId = usuarioCargoDto.EmployeeRoleId,
                UsuarioId = usuarioCargoDto.UsuarioId,
                CargoId = usuarioCargoDto.CargoId
            };

            _usuarioCargoDao.Update(usuarioCargo);
            return Ok(usuarioCargo);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployeeRole(int id)
        {
            var existingRole = _usuarioCargoDao.ReadById(id);
            if (existingRole == null) return NotFound();

            _usuarioCargoDao.Delete(id);
            return NoContent();
        }
    }

    // DTOs
    public class UsuarioCargoDto
    {
        public int EmployeeRoleId { get; set; }
        public int? UsuarioId { get; set; }
        public int? CargoId { get; set; }
    }
}
