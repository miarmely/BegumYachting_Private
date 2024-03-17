using BegumYatch.Core.Configs;
using BegumYatch.Core.DTOs.AdminPanel.Login;
using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BegumYatch.Service.Services
{
	public partial class LoginService : ILoginService 
	{
		private readonly IRoleService _roleService;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUserService _userService;
		private readonly IEmailService _emailService;
		private readonly JwtSettingsConfig _jwtSettingsConfig;

		public LoginService(
			IRoleService roleService,
			UserManager<AppUser> userManager,
			IUserService userService,
			IEmailService emailService,
			JwtSettingsConfig jwtSettingsConfig)
		{
			_roleService = roleService;
			_userManager = userManager;
			_userService = userService;
			_emailService = emailService;
			_jwtSettingsConfig = jwtSettingsConfig;
		}


		private async Task<string> LoginAsync(UserLoginDto userDto, params Roles[] validRoles)
		{
			#region security control (THROW)

			#region when email is wrong (THROW)
			var user = await _userManager.FindByEmailAsync(userDto.Email);

			if (user == null)
				throw new MiarException(
					404,
					"AE",
					"Authentication Error",
					"email veya şifre yanlış");
			#endregion

			#region when password is wrong (THROW)
			var isPasswordTrue = await _userManager
				.CheckPasswordAsync(user, userDto.Password);

			if (!isPasswordTrue)
				throw new MiarException(
					404,
					"AE",
					"Authentication Error",
					"email veya şifre yanlış");
			#endregion

			#region when user roles is invalid (THROW)
			var userRoles = await _roleService.GetUserRolesAsync(user.Id);

			if (!await IsUserRolesValidAsync(userRoles, validRoles))
				throw new MiarException(
					404,
					"AE",
					"Authentication Error",
					"email veya şifre yanlış");
			#endregion

			#endregion

			return await GenerateTokenForUserAsync(
				user,
				await _roleService.GetUserRolesAsync(user.Id));
		}

		private async Task<bool> IsUserRolesValidAsync(
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

		private async Task<string> GenerateTokenForUserAsync(
			AppUser user,
			IEnumerable<MiarRole> userRoles)
		{
			#region set signingCredentials
			var secretKeyInBytes = Encoding.UTF8
				.GetBytes(_jwtSettingsConfig.SecretKey);

			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(secretKeyInBytes),
				SecurityAlgorithms.HmacSha256);
			#endregion

			#region set jwt token
			var token = new JwtSecurityToken(
				_jwtSettingsConfig.ValidIssuer,
				_jwtSettingsConfig.ValidAudience1,
				await GenerateClaimsAsync(user, userRoles),
				signingCredentials: signingCredentials);
			#endregion

			return new JwtSecurityTokenHandler()
				.WriteToken(token);
		}

		private async Task<IEnumerable<Claim>> GenerateClaimsAsync(
			AppUser user,
			IEnumerable<MiarRole> userRoles)
		{
			var claims = new Collection<Claim>
			{
				new (MiarClaimTypes.Id, user.Id.ToString()),
				new (MiarClaimTypes.NameSurname, user.NameSurname),
				new (MiarClaimTypes.PhoneNumber, user.PhoneNumber),
				new (MiarClaimTypes.Email, user.Email),
				new (MiarClaimTypes.Gender, user.Gender),
				new (MiarClaimTypes.Nationality, user.Nationality),
				new (MiarClaimTypes.YachtType, user.YacthType.ToString()),
				new (MiarClaimTypes.YachtName, user.YacthName),
				new (MiarClaimTypes.Flag, user.Flag),
				new (MiarClaimTypes.NewPassportNo, user.NewPassportNo),
				new (MiarClaimTypes.OldPassportNo, user.OldPassportNo),
				new (MiarClaimTypes.Rank, user.Rank),
				new (MiarClaimTypes.DateOfIssue, user.DateOfIssue.ToString()),
				new (MiarClaimTypes.PassPortExpiry, user.PassPortExpiry.ToString()),
				new (MiarClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
				new (MiarClaimTypes.PlaceOfBirth, user.PlaceOfBirth),
				new (MiarClaimTypes.IsPersonel, user.IsPersonel.ToString()),
				new (MiarClaimTypes.IsDeleted, user.IsDeleted.ToString()),
			};

			#region add user roles to claims
			// when user has roles
			if (userRoles.Count() > 0)
				foreach (var userRole in userRoles)
					claims.Add(new Claim(
						MiarClaimTypes.RoleName,
						userRole.RoleName));

			// when user hasn't roles
			else
				claims.Add(new Claim(
					MiarClaimTypes.RoleName,
					"null"));
			#endregion

			return claims;
		}
	}  // private


	public partial class LoginService  // public
	{
		public async Task<object> LoginForMobileAsync(UserLoginDto userDto)
		{
			var token = await LoginAsync(userDto, Roles.User, Roles.Admin);

			return token;
		}

		public async Task LoginForPanelAsync(UserLoginDto userDto)
		{
			var token = LoginAsync(userDto, Roles.Admin);
		}

		public async Task SendCodeToMailForResetPasswordAsync(
			LoginParamsForSendCodeToMail loginParams,
			params Roles[] validRoles)
		{
			#region when user not found (THROW)
			var user = await _userManager.FindByEmailAsync(loginParams.Email);

			if (user == null)
				throw new MiarException(
					404,
					"NF-U",
					"Not Found - User",
					"kullanıcı bulunamadı");
			#endregion

			#region when user roles is invalid (THROW)
			var userRoles = await _roleService.GetUserRolesAsync(user.Id);

			if (!await IsUserRolesValidAsync(userRoles, validRoles))
				throw new MiarException(
					404,
					"NF-U",
					"Not Found - User",
					"kullanıcı bulunamadı");
			#endregion

			#region set verify code with 6 digit
			var random = new Random();
			var digitCount = 6;
			var verifyCode = "";

			for (int repeat = 0; repeat < digitCount; repeat++)
			{
				verifyCode += random
					.Next(10)  // numbers smaller than 10
					.ToString();
			}
			#endregion

			#region set mail otp
			string tokenForResetPassword = await _userManager
				.GeneratePasswordResetTokenAsync(user);

			var model = new MailOtp
			{
				UserId = user.Id,
				Email = user.Email,
				Code = verifyCode,
				DateAdded = DateTime.Now,
				Token = tokenForResetPassword
			};
			#endregion

			#region send mail
			var mailMessage = $@"<p style='color: black'>Şifrenizi resetlemek için lütfen bu doğrulama kodunu kullanın:</p>
                <p><b style='font-size:20px'>{verifyCode}</b></p>
                <p><b style='font-size:14.5px'>Begum Yachting</b></p>";

			try
			{
				await _userService.SendOtp(model);
				await _emailService.SendEmailAsync(
					"MostIdea - Forget Password",
					mailMessage,
					user.Email);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			#endregion
		}

		public async Task<object> VerifyCodeForResetPasswordAsync(
			LoginParamsForVerifyCode loginParams)
		{
			#region when user not found (THROW)
			var user = await _userManager.FindByEmailAsync(loginParams.Email);

			if (user == null)
				throw new MiarException(
					400,
					"NF-U",
					"Not Found - User",
					"kullanıcı bulunamadı");
			#endregion

			#region verify the code (THROW)
			var token = await _userService.VerifyOtp(loginParams.VerificationCode, user.Id);

			// when code is wrog
			if (token == "")
				throw new MiarException(
					404,
					"FP-VC",
					"Forgot Password - Verify Code",
					"doğrulama kodu yanlış");

			#endregion

			return new
			{
				UserId = user.Id,
				TokenForResetPassword = token
			};
		}

		public async Task ResetPasswordAsync(
			LoginDtoForResetPassword loginDto)
		{
			#region reset password of user
			var user = await _userManager.FindByIdAsync(loginDto.UserId);

			var result = await _userManager.ResetPasswordAsync(
				user,
				loginDto.TokenForResetPassword,
				loginDto.NewPassword);
			#endregion

			#region when any error is occured (THROW)
			if (!result.Succeeded)
				throw new MiarException(
					500,
					"ISE",
					"Internal Server Error",
					"şifre resetlenirken bir hata oluştu");
			#endregion
		}
	}
}