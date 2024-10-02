using Microsoft.AspNetCore.Authorization;

namespace _6D.Authorization
{
    /// <summary>
    /// Represents a requirement for a specific permission.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets the key of the required permission.
        /// </summary>
        public string PermissionKey { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequirement"/> class.
        /// </summary>
        /// <param name="permissionKey">The key of the required permission.</param>
        public PermissionRequirement(string permissionKey)
        {
            PermissionKey = permissionKey;
        }
    }
}