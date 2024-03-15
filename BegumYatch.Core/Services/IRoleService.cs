namespace BegumYatch.Core.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetAllRoleNamesAsync();
    }
}
