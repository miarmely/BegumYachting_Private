using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.AdminPanel;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;

namespace BegumYatch.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Temp> _repository;

        public RoleService(IGenericRepository<Temp> repository)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<string>> GetAllRoleNamesAsync()
        {
            #region get role names
            var roles = await _repository
                .FromSqlRawAsync<MiarRole>("SELECT * FROM Roles");

            var roleNames = roles.Select(r => r.RoleName);
            #endregion

            return roleNames;
        }

        public async Task<IEnumerable<MiarRole>> GetUserRolesAsync(int userId) =>
            await _repository
                .FromSqlRawAsync<MiarRole>(
                    "EXEC Role_GetRolesOfUser @UserId = {0}",
                    userId);

		public async Task<bool> IsUserRolesValidAsync(
			IEnumerable<MiarRole> userRoles,
			Roles[] validRoles)
		{
			#region when user roles is invalid
			if (!userRoles.Any(ur =>
					validRoles.Any(vr => vr.ToString() == ur.RoleName)))
				return false;
			#endregion

			return true;
		}
	}
}