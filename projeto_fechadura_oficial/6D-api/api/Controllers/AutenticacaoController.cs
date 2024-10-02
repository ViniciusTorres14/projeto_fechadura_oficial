using Microsoft.AspNetCore.Mvc;
using _6D.DAO;
using _6D.Models;

namespace _6D.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AutenticacaoController : ControllerBase
	{
		private readonly AutenticacaoDAO _authDao;

		public AutenticacaoController(AutenticacaoDAO authDao)
		{
			_authDao = authDao;
		}

		[HttpPost("register")]
		public IActionResult Register([FromBody] AutenticacaoDAO.RegisterDto registerDto)
		{
			if (registerDto == null)
				return BadRequest("Invalid client request.");

			if (_authDao.UserExists(registerDto.Username))
				return BadRequest("User already exists.");

			var token = _authDao.Register(registerDto);
			return Ok(new { Token = token });
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDto loginDto)
		{
			if (loginDto == null)
				return BadRequest("Invalid client request.");

			var token = _authDao.Login(loginDto.Username, loginDto.Password);
			if (token == null)
				return Unauthorized("Invalid username or password.");

			return Ok(new { Token = token });
		}

		public class LoginDto
		{
			[Required]
			public string Username { get; set; }

			[Required]
			public string Password { get; set; }
		}
	}
}
