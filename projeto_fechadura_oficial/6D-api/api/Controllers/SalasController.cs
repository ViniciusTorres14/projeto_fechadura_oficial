namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalasController : ControllerBase
    {
        private readonly SalasDAO _salasDao;

        public SalasController(SalasDAO salasDao)
        {
            _salasDao = salasDao;
        }

        [HttpGet]
        public IActionResult GetAllSalas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var salas = _salasDao.ReadAll(pageNumber, pageSize);
            var totalCount = _salasDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, Salas = salas });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetSalaById(int id)
        {
            var sala = _salasDao.ReadById(id);
            if (sala == null) return NotFound();
            return Ok(sala);
        }

        [HttpPost]
        public IActionResult CreateSala([FromBody] CreateSalaDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var sala = new Sala
            {
                Nome = createDto.Nome,
                Descricao = createDto.Descricao,
                Status = createDto.Status,
                Imagem = !string.IsNullOrEmpty(createDto.ImageBase64) ? Convert.FromBase64String(createDto.ImageBase64) : null,
                OcupadoPorUsuarioId = createDto.OcupadoPorUsuarioId
            };

            _salasDao.Create(sala);

            var salaDto = new SalaDto
            {
                SalaId = sala.SalaId,
                Nome = sala.Nome,
                Descricao = sala.Descricao,
                Status = sala.Status,
                ImageBase64 = sala.Imagem != null ? Convert.ToBase64String(sala.Imagem) : null,
                OcupadoPorUsuarioId = sala.OcupadoPorUsuarioId,
                OccupiedByEmployeeName = _salasDao.GetEmployeeName(sala.OcupadoPorUsuarioId)
            };

            return CreatedAtAction(nameof(GetSalaById), new { id = sala.SalaId }, salaDto);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateSala(int id, [FromBody] UpdateSalaDto updateDto)
        {
            if (id != updateDto.SalaId) return BadRequest("ID mismatch.");
            if (_salasDao.ReadById(id) == null) return NotFound();

            var sala = new Sala
            {
                SalaId = updateDto.SalaId,
                Nome = updateDto.Nome,
                Descricao = updateDto.Descricao,
                Status = updateDto.Status,
                Imagem = !string.IsNullOrEmpty(updateDto.ImageBase64) ? Convert.FromBase64String(updateDto.ImageBase64) : null,
                OcupadoPorUsuarioId = updateDto.OcupadoPorUsuarioId
            };

            _salasDao.Update(sala);
            return Ok(sala);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRoom(int id)
        {
            if (_salasDao.ReadById(id) == null) return NotFound();

            _salasDao.Delete(id);
            return NoContent();
        }
    }

    // DTOs
    public class SalaDto
    {
        public int SalaId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public bool Status { get; set; }
        public string? ImageBase64 { get; set; }
        public int? OcupadoPorUsuarioId { get; set; }
        public string? OccupiedByEmployeeName { get; set; }
    }

    public class CreateSalaDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Required]
        public bool Status { get; set; }

        public string? ImageBase64 { get; set; }

        public int? OcupadoPorUsuarioId { get; set; }
    }

    public class UpdateSalaDto
    {
        [Required]
        public int SalaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Required]
        public bool Status { get; set; }

        public string? ImageBase64 { get; set; }

        public int? OcupadoPorUsuarioId { get; set; }
    }
}
