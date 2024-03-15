using BegumYatch.Core.DTOs.AdminPanel.Login;
using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;
using Microsoft.AspNetCore.Http;

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
        Task<object> LoginAsync(UserLoginDto userDto);

        Task SendCodeToMailForResetPasswordAsync(
            LoginParamsForSendCodeToMail loginParams);

        Task<object> VerifyCodeForResetPasswordAsync(
            LoginParamsForVerifyCode loginParams);

        Task ResetPasswordAsync(LoginDtoForResetPassword loginDto);
        Task<List<MiarUser>> GetAllUsers(int accountId);

        Task<List<MiarUser>> GetUsersByFilteringAsync(
            int? UserId = null,
            string? Email = null,
            string? Phone = null,
            bool CheckIsDeleted = true);

        Task CreateUserAsync(UserDtoForCreate userDto, Roles role);

        Task UpdateUserAsync(
            string email,
            UserDtoForUpdate userDto,
            HttpContext context);

        Task DeleteUsersAsync(UserDtoForDelete userDto);
        Task<IEnumerable<string>> GetAllRoleNamesAsync();
    }
}
