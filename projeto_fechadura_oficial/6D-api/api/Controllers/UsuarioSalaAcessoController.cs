namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioSalaAcessoController : ControllerBase
    {
        private readonly UsuarioSalaAcessoDAO _usuarioSalaAcessoDao;

        public UsuarioSalaAcessoController(UsuarioSalaAcessoDAO usuarioSalaAcessoDao)
        {
            _usuarioSalaAcessoDao = usuarioSalaAcessoDao;
        }

        [HttpGet]
        public IActionResult GetAllEmployeeRoomAccesses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var accesses = _usuarioSalaAcessoDao.ReadAll(pageNumber, pageSize);
            var totalCount = _usuarioSalaAcessoDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, Accesses = accesses });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployeeRoomAccessById(int id)
        {
            var access = _usuarioSalaAcessoDao.ReadById(id);
            if (access == null) return NotFound();
            return Ok(access);
        }

        [HttpPost]
        public IActionResult CreateEmployeeRoomAccess([FromBody] UsuarioSalaAcessoDto accessDto)
        {
            if (accessDto == null) return BadRequest();

            var access = new UsuarioSalaAcesso
            {
                UsuarioId = accessDto.UsuarioId,
                SalaId = accessDto.SalaId
            };

            _usuarioSalaAcessoDao.Create(access);
            return CreatedAtAction(nameof(GetEmployeeRoomAccessById), new { id = access.AcessoId }, access);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateEmployeeRoomAccess(int id, [FromBody] UsuarioSalaAcessoDto accessDto)
        {
            if (id != accessDto.AcessoId) return BadRequest("ID mismatch.");
            var existingAccess = _usuarioSalaAcessoDao.ReadById(id);
            if (existingAccess == null) return NotFound();

            var access = new UsuarioSalaAcesso
            {
                AcessoId = accessDto.AcessoId,
                UsuarioId = accessDto.UsuarioId,
                SalaId = accessDto.SalaId
            };

            _usuarioSalaAcessoDao.Update(access);
            return Ok(access);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployeeRoomAccess(int id)
        {
            var existingAccess = _usuarioSalaAcessoDao.ReadById(id);
            if (existingAccess == null) return NotFound();

            _usuarioSalaAcessoDao.Delete(id);
            return NoContent();
        }
    }

    // DTOs
    public class UsuarioSalaAcessoDto
    {
        public int AcessoId { get; set; }
        public int? UsuarioId { get; set; }
        public int? SalaId { get; set; }
    }
}
