using Microsoft.AspNetCore.Mvc;
using _6D.DAO;
using _6D.Models;
using System.Threading.Tasks;

namespace _6D.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly UsuariosDAO _usuariosDao;
        private readonly UsuarioSalaAcessoDAO _accessDao;
        private readonly RegistroDeAcessoDAO _logsDao;

        public AccessController(UsuariosDAO usuariosDao, UsuarioSalaAcessoDAO accessDao, RegistroDeAcessoDAO logsDao)
        {
            _usuariosDao = usuariosDao;
            _accessDao = accessDao;
            _logsDao = logsDao;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestAccess([FromBody] AccessRequest request)
        {
            if (request == null || (string.IsNullOrEmpty(request.RFID) && string.IsNullOrEmpty(request.PinCode)))
            {
                return BadRequest("RFID ou PinCode devem ser fornecidos.");
            }

            Usuario? usuario = null;

            if (!string.IsNullOrEmpty(request.RFID))
            {
                usuario = _usuariosDao.GetEmployeeByRFIDTag(request.RFID);
            }
            else if (!string.IsNullOrEmpty(request.PinCode))
            {
                usuario = _usuariosDao.GetEmployeeByPinCode(request.PinCode);
            }

            if (usuario == null)
            {
                // Log de acesso negado
                await _logsDao.CreateAsync(new RegistrosDeAcesso
                {
                    UsuarioId = null,
                    SalaId = request.RoomId,
                    AccessTime = DateTime.UtcNow,
                    AccessGranted = false
                });

                return Unauthorized(new { message = "Usuário não encontrado ou credenciais inválidas." });
            }

            bool hasAccess = _accessDao.UserHasAccessToRoom(usuario.UsuarioId, request.RoomId);

            // Log do acesso
            await _logsDao.CreateAsync(new RegistrosDeAcesso
            {
                UsuarioId = usuario.UsuarioId,
                SalaId = request.RoomId,
                AccessTime = DateTime.UtcNow,
                AccessGranted = hasAccess
            });

            if (hasAccess)
            {
                return Ok(new { message = "Acesso concedido." });
            }
            else
            {
                return StatusCode(403, new { message = "Acesso negado para esta sala." });
            }
        }
    }
}