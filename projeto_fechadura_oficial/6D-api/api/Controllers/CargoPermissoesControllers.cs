namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargoPermissoesController : ControllerBase
    {
        private readonly CargoPermissoesDAO _cargoPermissoesDao;
        private readonly CargosDAO _cargosDao;
        private readonly PermissoesDAO _permissoesDao;

        public CargoPermissoesController(CargoPermissoesDAO cargoPermissoesDao, CargosDAO cargosDao, PermissoesDAO permissoesDao)
        {
            _cargoPermissoesDao = cargoPermissoesDao;
            _cargosDao = cargosDao;
            _permissoesDao = permissoesDao;
        }

        [HttpGet]
        public IActionResult GetAllCargoPermissions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var cargoPermissoes = _cargoPermissoesDao.ReadAll(pageNumber, pageSize);
            var totalCount = _cargoPermissoesDao.Count();
            return Ok(new { totalCount, pageNumber, pageSize, CargoPermissoes = cargoPermissoes });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCargoPermissionById(int id)
        {
            var cargoPermissao = _cargoPermissoesDao.ReadById(id);
            if (cargoPermissao == null) return NotFound();
            return Ok(cargoPermissao);
        }

        [HttpPost]
        public IActionResult CreateCargoPermissao([FromBody] CreateCargoPermissaoDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validate existence of Cargo and Permissao
            if (_cargosDao.ReadById(createDto.CargoId) == null || _permissoesDao.ReadById(createDto.PermissaoId) == null)
                return BadRequest("Invalid CargoId or PermissaoId.");

            var cargoPermissao = new CargoPermissao
            {
                CargoId = createDto.CargoId,
                PermissaoId = createDto.PermissaoId
            };

            _cargoPermissoesDao.Create(cargoPermissao);

            var cargoPermissaoDto = new CargoPermissaoDto
            {
                CargoPermissaoId = cargoPermissao.CargoPermissaoId,
                CargoId = cargoPermissao.CargoId,
                RoleName = _cargosDao.ReadById(cargoPermissao.CargoId)?.RoleName,
                PermissaoId = cargoPermissao.PermissaoId,
                PermissionKey = _permissoesDao.ReadById(cargoPermissao.PermissaoId)?.PermissionKey
            };

            return CreatedAtAction(nameof(GetCargoPermissionById), new { id = cargoPermissao.CargoPermissaoId }, cargoPermissaoDto);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCargoPermissao(int id, [FromBody] UpdateCargoPermissaoDto updateDto)
        {
            if (id != updateDto.CargoPermissaoId) return BadRequest("ID mismatch.");
            if (_cargoPermissoesDao.ReadById(id) == null) return NotFound();

            // Validate existence of Cargo and Permissao
            if (_cargosDao.ReadById(updateDto.CargoId) == null || _permissoesDao.ReadById(updateDto.PermissaoId) == null)
                return BadRequest("Invalid CargoId or PermissaoId.");

            var cargoPermissao = new CargoPermissao
            {
                CargoPermissaoId = updateDto.CargoPermissaoId,
                CargoId = updateDto.CargoId,
                PermissaoId = updateDto.PermissaoId
            };

            _cargoPermissoesDao.Update(cargoPermissao);
            return Ok(cargoPermissao);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRolePermission(int id)
        {
            if (_cargoPermissoesDao.ReadById(id) == null) return NotFound();

            _cargoPermissoesDao.Delete(id);
            return NoContent();
        }
    }

    // DTOs
    public class CargoPermissaoDto
    {
        public int CargoPermissaoId { get; set; }
        public int CargoId { get; set; }
        public string? RoleName { get; set; }
        public int PermissaoId { get; set; }
        public string? PermissionKey { get; set; }
    }

    public class CreateCargoPermissaoDto
    {
        [Required]
        public int CargoId { get; set; }

        [Required]
        public int PermissaoId { get; set; }
    }

    public class UpdateCargoPermissaoDto
    {
        [Required]
        public int CargoPermissaoId { get; set; }

        [Required]
        public int CargoId { get; set; }

        [Required]
        public int PermissaoId { get; set; }
    }
}
