namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrosDeAcessoController : ControllerBase
    {
        private readonly RegistroDeAcessoDAO _registrosDeAcessoDao;

        public RegistrosDeAcessoController(RegistroDeAcessoDAO registrosDeAcessoDao)
        {
            _registrosDeAcessoDao = registrosDeAcessoDao;
        }

        [HttpGet]
        public IActionResult GetAllAccessLogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var registrosDeAcesso = _registrosDeAcessoDao.ReadAll(pageNumber, pageSize);
            var totalCount = _registrosDeAcessoDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, RegistrosDeAcesso = registrosDeAcesso });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAccessLogById(int id)
        {
            var registroDeAcesso = _registrosDeAcessoDao.ReadById(id);
            if (registroDeAcesso == null) return NotFound();
            return Ok(registroDeAcesso);
        }

        [HttpPost]
        public IActionResult CreateAccessLog([FromBody] RegistrosDeAcesso registroDeAcesso)
        {
            if (registroDeAcesso == null) return BadRequest();
            _registrosDeAcessoDao.Create(registroDeAcesso);
            return CreatedAtAction(nameof(GetAccessLogById), new { id = registroDeAcesso.RegistroId }, registroDeAcesso);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateAccessLog(int id, [FromBody] RegistrosDeAcesso registroDeAcesso)
        {
            if (id != registroDeAcesso.RegistroId) return BadRequest("ID mismatch.");
            var existingLog = _registrosDeAcessoDao.ReadById(id);
            if (existingLog == null) return NotFound();

            _registrosDeAcessoDao.Update(registroDeAcesso);
            return Ok(registroDeAcesso);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteAccessLog(int id)
        {
            var existingLog = _registrosDeAcessoDao.ReadById(id);
            if (existingLog == null) return NotFound();

            _registrosDeAcessoDao.Delete(id);
            return NoContent();
        }
    }
}
