using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.Role;

namespace BegumYatch.Core.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetAllRoleNamesAsync();
        Task<IEnumerable<MiarRole>> GetUserRolesAsync(int userId);

		Task<bool> IsUserRolesValidAsync(
			IEnumerable<MiarRole> userRoles,
			Roles[] validRoles);
	}
}