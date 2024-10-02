using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _6D.DAO;

namespace _6D.Authorization
{
    /// <summary>
    /// Handles authorization based on user permissions.
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UsuariosDAO _usuariosDAO;
        private readonly UsuarioCargoDAO _usuarioCargoDAO;
        private readonly CargoPermissoesDAO _cargoPermissoesDAO;
        private readonly PermissoesDAO _permissoesDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionHandler"/> class.
        /// </summary>
        /// <param name="usuariosDAO">DAO for user operations.</param>
        /// <param name="usuarioCargoDAO">DAO for user-role operations.</param>
        /// <param name="cargoPermissoesDAO">DAO for role-permission operations.</param>
        /// <param name="permissoesDAO">DAO for permission operations.</param>
        public PermissionHandler(
            UsuariosDAO usuariosDAO,
            UsuarioCargoDAO usuarioCargoDAO,
            CargoPermissoesDAO cargoPermissoesDAO,
            PermissoesDAO permissoesDAO)
        {
            _usuariosDAO = usuariosDAO;
            _usuarioCargoDAO = usuarioCargoDAO;
            _cargoPermissoesDAO = cargoPermissoesDAO;
            _permissoesDAO = permissoesDAO;
        }

        /// <summary>
        /// Evaluates whether the requirement is satisfied.
        /// </summary>
        /// <param name="context">Authorization context.</param>
        /// <param name="requirement">Permission requirement.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Retrieve the employee ID from claims
            var employeeIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out int employeeId))
            {
                return Task.CompletedTask; // User is not authenticated or invalid employee ID
            }

            // Get the user
            var usuario = _usuariosDAO.ReadById(employeeId);
            if (usuario == null)
            {
                return Task.CompletedTask; // User does not exist
            }

            // Get roles associated with the user
            var userRoles = _usuarioCargoDAO.ReadByEmployeeId(employeeId)
                                            .Select(ur => ur.CargoId)
                                            .ToList();

            if (!userRoles.Any())
            {
                return Task.CompletedTask; // User has no roles
            }

            // Get permissions associated with the user's roles
            var rolePermissions = _cargoPermissoesDAO.Read()
                                                      .Where(rp => userRoles.Contains(rp.CargoId))
                                                      .Select(rp => rp.PermissaoId)
                                                      .Distinct()
                                                      .ToList();

            if (!rolePermissions.Any())
            {
                return Task.CompletedTask; // Roles have no permissions
            }

            // Get permission keys
            var permissionKeys = _permissoesDAO.Read()
                                              .Where(p => rolePermissions.Contains(p.PermissaoId))
                                              .Select(p => p.PermissionKey)
                                              .ToList();

            // Check if any permission matches the requirement
            if (permissionKeys.Contains(requirement.PermissionKey))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}