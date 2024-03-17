using BegumYatch.Core.DTOs.AdminPanel.Login;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Services;
using Microsoft.AspNetCore.Mvc;


namespace BegumYatch.API.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class LoginController : ControllerBase
	{
		private readonly ILoginService _loginService;

		public LoginController(ILoginService loginService)
		{
			_loginService = loginService;
		}


		[HttpPost("panel")]
		public async Task<IActionResult> LoginForPanel(
			[FromBody] UserLoginDto userDto)
		{
			var token = await _loginService.LoginForPanelAsync(userDto);

			return Ok(new
			{
				Token = token
			});
		}


		[HttpPost("mobile")]
		public async Task<IActionResult> LoginForMobile(
			[FromBody] UserLoginDto userDto)
		{
			var token = await _loginService.LoginForMobileAsync(userDto);

			return Ok(new
			{
				Token = token
			});
		}


		[HttpGet("forgotPassword/sendCodeToMail")]
		public async Task<IActionResult> SendCodeToMailForResetPassword(
			[FromQuery] LoginParamsForSendCodeToMail loginParams)
		{
			await _loginService.SendCodeToMailForResetPasswordAsync(
				loginParams);

			return NoContent();
		}


		[HttpGet("forgotPassword/verifyCode")]
		public async Task<IActionResult> VerifyCodeForResetPassword(
			[FromQuery] LoginParamsForVerifyCode loginParams)
		{
			var response = await _loginService
				.VerifyCodeForResetPasswordAsync(loginParams);

			return Ok(response);  // "userId" and "token" is exists in response.
		}


		[HttpPost("forgotPassword/resetPassword")]
		public async Task<IActionResult> MiarResetPassword(
			[FromBody] LoginDtoForResetPassword loginDto)
		{
			await _loginService.ResetPasswordAsync(loginDto);

			return NoContent();
		}
	}
}