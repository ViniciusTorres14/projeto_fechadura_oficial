namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly CargosDAO _cargosDao;

        public CargosController(CargosDAO cargosDao)
        {
            _cargosDao = cargosDao;
        }

        [HttpGet]
        public IActionResult GetAllCargos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var cargos = _cargosDao.ReadAll(pageNumber, pageSize);
            var totalCount = _cargosDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, Cargos = cargos });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCargoById(int id)
        {
            var cargo = _cargosDao.ReadById(id);
            if (cargo == null) return NotFound();
            return Ok(cargo);
        }

        [HttpPost]
        public IActionResult CreateCargo([FromBody] CreateCargoDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cargo = new Cargo
            {
                RoleName = createDto.RoleName,
                Descricao = createDto.Descricao
            };

            _cargosDao.Create(cargo);

            var cargoDto = new CargoDto
            {
                CargoId = cargo.CargoId,
                RoleName = cargo.RoleName,
                Descricao = cargo.Descricao
            };

            return CreatedAtAction(nameof(GetCargoById), new { id = cargo.CargoId }, cargoDto);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCargo(int id, [FromBody] UpdateCargoDto updateDto)
        {
            if (id != updateDto.CargoId) return BadRequest("ID mismatch.");
            if (_cargosDao.ReadById(id) == null) return NotFound();

            var cargo = new Cargo
            {
                CargoId = updateDto.CargoId,
                RoleName = updateDto.RoleName,
                Descricao = updateDto.Descricao
            };

            _cargosDao.Update(cargo);
            return Ok(cargo);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRole(int id)
        {
            if (_cargosDao.ReadById(id) == null) return NotFound();

            _cargosDao.Delete(id);
            return NoContent();
        }
    }

    // DTOs
    public class CargoDto
    {
        public int CargoId { get; set; }
        public string RoleName { get; set; }
        public string? Descricao { get; set; }
    }

    public class CreateCargoDto
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public string? Descricao { get; set; }
    }

    public class UpdateCargoDto
    {
        [Required]
        public int CargoId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public string? Descricao { get; set; }
    }
}
