using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;

namespace BegumYatch.Core.Services
{
    public partial interface IUserService : IService<AppUser>
    {
        Task<GetPersonelInfoByIdDto> GetPersonelInfo(int id);
        Task<List<ReturnResponseModel>> IsChangeEmail(SendConfirmCodeUpdateEmail sendConfirmCodeUpdateEmailDto);
        Task<List<ReturnResponseModel>> VerifyConfirmCode(VerifyConfirmCode verifyConfirmCode);
        Task<String> SendOtp(MailOtp model);
        Task<String> VerifyOtp(String code, int UserId);
        Task<List<TDto>> GetAllUsers<TDto>();
    }

    public partial interface IUserService  // By MERT
    {
        Task<string> LoginAsync(UserLoginDto userDto);
        Task CreateUserAsync(UserDtoForCreate userDto);
        Task UpdateUserAsync(string email, UserDtoForUpdate userDto);
        Task DeleteUsersAsync(UserDtoForDelete userDto);

        Task<List<MiarUser>> GetUsersByFilteringAsync(
            int? UserId = null,
            string? Email = null,
            string? Phone = null,
            bool CheckIsDeleted = true);
    }
}
