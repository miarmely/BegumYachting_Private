using BegumYatch.Core.DTOs.AdminPanel.Login;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;
using Microsoft.AspNetCore.Http;

namespace BegumYatch.Core.Services
{
	public interface ILoginService
	{
		Task<string> LoginAsync(UserLoginDto userDto, params Roles[] validRoles);

		Task SendCodeToMailForResetPasswordAsync(
			LoginParamsForSendCodeToMail loginParams,
			params Roles[] validRoles);

		Task<object> VerifyCodeForResetPasswordAsync(
			LoginParamsForVerifyCode loginParams);

		Task ResetPasswordAsync(LoginDtoForResetPassword loginDto);

		Task<string> GenerateTokenForUserAsync(AppUser user, string role);
	}
}