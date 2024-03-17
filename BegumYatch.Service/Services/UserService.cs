using AutoMapper;
using BegumYatch.Core.Configs;
using BegumYatch.Core.DTOs.AdminPanel.Login;
using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BegumYatch.Service.Services
{
	public partial class UserService : Service<AppUser>, IUserService
	{
		private readonly IGenericRepository<AppUser> _userRepository;
		private readonly IGenericRepository<MailOtp> _mailOtpRepository;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UserService(
			IGenericRepository<AppUser> userRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IEmailService emailService,
			UserManager<AppUser> userManager,
			IGenericRepository<MailOtp> mailOtpRepository,
			IOptions<JwtSettingsConfig> jwtSettingsConfig,
			IRoleService roleService)
			: base(userRepository, unitOfWork)
		{
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_emailService = emailService;
			_userManager = userManager;
			_mailOtpRepository = mailOtpRepository;
			_jwtSettingsConfig = jwtSettingsConfig.Value;
			_roleService = roleService;
		}

		public async Task<List<ReturnResponseModel>> VerifyConfirmCode(VerifyConfirmCode verifyConfirmCode)
		{
			List<ReturnResponseModel> allReponses = new List<ReturnResponseModel>();
			var currentUser = _userRepository.Where(x => x.Id == verifyConfirmCode.Id).FirstOrDefault();
			if (currentUser == null)
				throw new Exception("There is no user with this information.");
			if (currentUser.ConfirmCode == verifyConfirmCode.ConfirmCode)
			{
				currentUser.Email = verifyConfirmCode.Email;
				currentUser.EmailConfirmed = true;

				if (currentUser.Flag != verifyConfirmCode.Flag || currentUser.YacthName != verifyConfirmCode.YacthName || currentUser.YacthType != verifyConfirmCode.YacthType)
				{
					if (currentUser.IsUpdated)
					{
						var returnInfo = new ReturnResponseModel
						{
							ReturnMessage = "Değişiklik yapamazsınız."
						};
						allReponses.Add(returnInfo);
					}
					else
					{
						currentUser.IsUpdated = true;
						currentUser.Flag = verifyConfirmCode.Flag;
						currentUser.YacthType = verifyConfirmCode.YacthType;
						currentUser.YacthName = verifyConfirmCode.YacthName;
						_userRepository.Update(currentUser);
						await _unitOfWork.CommitAsync();

						var returnInfo = new ReturnResponseModel
						{
							ReturnMessage = "Bilgiler başarıyla değiştirilmiştir."
						};
						allReponses.Add(returnInfo);
					}
					_userRepository.Update(currentUser);
					await _unitOfWork.CommitAsync();
				}
				else
				{
					_userRepository.Update(currentUser);
					await _unitOfWork.CommitAsync();
					var returnInfo = new ReturnResponseModel
					{
						ReturnMessage = "Emailiniz " + $"{verifyConfirmCode.Email}" + " olarak değiştirilmiştir"
					};
					allReponses.Add(returnInfo);
				}

			}
			else
			{
				var returnInfo = new ReturnResponseModel
				{
					ReturnMessage = "Onay kodlarınız uyuşmamaktadır."
				};
				allReponses.Add(returnInfo);
			}
			return allReponses;
		}

		public async Task<String> SendOtp(MailOtp model)
		{
			var entity = _mapper.Map<MailOtp>(model);

			await _mailOtpRepository.AddAsync(entity);
			await _unitOfWork.CommitAsync();

			return "Eklendi";
		}

		public async Task<String> VerifyOtp(String code, int userId)
		{
			var aaa = _mailOtpRepository
				.GetAll()
				.Where(x => x.UserId == userId && x.Code == code)
				.ToList()
				.OrderByDescending(x => x.DateAdded);

			if (aaa.Any())
				return aaa.FirstOrDefault().Token;
			else
				return "";

		}

		public async Task<List<ReturnResponseModel>> IsChangeEmail(SendConfirmCodeUpdateEmail sendConfirmCodeUpdateEmailDto)
		{
			List<ReturnResponseModel> allReponses = new List<ReturnResponseModel>();
			var currentUser = _userRepository.Where(x => x.Id == sendConfirmCodeUpdateEmailDto.Id).FirstOrDefault();
			if (currentUser == null)
				throw new Exception("There is no user with this information.");
			if (currentUser.Email != sendConfirmCodeUpdateEmailDto.Email)
			{
				Random random = new Random();
				currentUser.ConfirmCode = random.Next(100000, 1000000);
				if (currentUser.IsUpdated)
				{
					var returnInfo = new ReturnResponseModel
					{
						ReturnMessage = "Değişiklik yapamazsınız."
					};
					allReponses.Add(returnInfo);
				}
				else
				{

					_userRepository.Update(currentUser);
					await _unitOfWork.CommitAsync();
					_emailService.SendEmailAsync("BegumYacht Email Verify", "Your confirmation code to change your email: " + $"{currentUser.ConfirmCode}", currentUser.Email);
					var returnInfo = new ReturnResponseModel
					{
						Id = currentUser.Id,
						ConfirmCode = currentUser.ConfirmCode,
						Email = sendConfirmCodeUpdateEmailDto.Email,
						YacthName = sendConfirmCodeUpdateEmailDto.YacthName,
						YacthType = sendConfirmCodeUpdateEmailDto.YacthType,
						Flag = sendConfirmCodeUpdateEmailDto.Flag,
						ReturnMessage = "Email değişikliği için " + $"{currentUser.Email} 'a " + "kod gönderildi."
					};
					allReponses.Add(returnInfo);
				}
			}
			else
			{
				if (currentUser.IsUpdated)
				{
					var returnInfo = new ReturnResponseModel
					{
						ReturnMessage = "Değişiklik yapamazsınız."
					};
					allReponses.Add(returnInfo);
				}
				else
				{
					currentUser.IsUpdated = true;
					currentUser.YacthName = sendConfirmCodeUpdateEmailDto.YacthName;
					currentUser.YacthType = sendConfirmCodeUpdateEmailDto.YacthType;
					currentUser.Flag = sendConfirmCodeUpdateEmailDto.Flag;
					_userRepository.Update(currentUser);
					await _unitOfWork.CommitAsync();

					var returnInfo = new ReturnResponseModel
					{
						Id = currentUser.Id,
						Email = currentUser.Email,
						Flag = currentUser.Flag,
						YacthName = currentUser.YacthName,
						YacthType = currentUser.YacthType,
						ReturnMessage = "Bilgiler başarıyla değiştirilmiştir."
					};
					allReponses.Add(returnInfo);
				}
			}
			return allReponses;
		}

		public async Task<GetPersonelInfoByIdDto> GetPersonelInfo(int id)
		{
			var personelInfo = await _userRepository.GetByIdAsync(id);

			if (personelInfo == null)
				throw new Exception("Böyle bir kullanıcı yoktur");

			else
			{
				var mapPersonelInfoDto = _mapper.Map<GetPersonelInfoByIdDto>(personelInfo);
				return mapPersonelInfoDto;
			}
		}

		public async Task<List<TDto>> GetAllUsers<TDto>()
		{
			var users = await _userRepository.GetAll().ToListAsync();
			var usersDto = _mapper.Map<List<TDto>>(users);

			if (usersDto != null && usersDto.Count > 0)
				return usersDto;

			else
				return null;
		}
	}  // By RUMEYSA


	public partial class UserService  // By MERT (PUBLIC)
	{
		#region CRUD
		public async Task CreateUserAsync(
			UserDtoForCreate userDto,
			Roles role)
		{
			await ControlConflictForUserAsync(
				userDto.Email,
				userDto.PhoneNumber);

			#region create user
			var user = _mapper.Map<AppUser>(userDto);
			user.UserName = userDto.Email;

			var result = await _userManager.CreateAsync(
				user,
				userDto.Password);
			#endregion

			#region save user role
			await _userRepository.ExecuteSqlRawAsync(
				"INSERT INTO UsersAndRoles VALUES ({0}, {1})",
				user.Id,
				role);
			#endregion


			#region when any error occured (error)
			if (!result.Succeeded)
				throw new MiarException(
					500,
					"ISE",
					"Internal Server Error",
					"kullanıcı eklenirken bir hata oluştu");
			#endregion
		}

		public async Task<List<MiarUser>> GetAllUsers(int accountId)
		{
			#region get users except account owner (THROW)
			var users = await _userRepository
				.FromSqlRawAsync<MiarUser>(
					"EXEC User_GetAll @AccountId={0}", accountId);

			// when any user not found
			if (users.Count == 0)
				throw new MiarException(
					404,
					"NF-U",
					"Not Found - User",
					"kullanıcı bulunamadı");
			#endregion

			return users;
		}

		public async Task<List<MiarUser>> GetUsersByFilteringAsync(
			int? UserId = null,
			string? Email = null,
			string? Phone = null,
			bool CheckIsDeleted = true)
		{
			#region get users by filtering
			var sql = "EXEC User_GetUsersByFiltering " +
				"@UserId = {0}, " +
				"@Email = {1}, " +
				"@Phone = {2}, " +
				"@CheckIsDeleted = {3}";

			var users = await _userRepository
				.FromSqlRawAsync<MiarUser>(
					sql,
					UserId,
					Email,
					Phone,
					CheckIsDeleted);
			#endregion

			#region when any user not found (THROW)
			if (users.Count == 0)
				throw new MiarException(
					404,
					"NF-U",
					"Not Found - User",
					"kullanıcı bulunamadı");
			#endregion

			return users;
		}

		public async Task<string> UpdateUserAsync(
			string email,
			UserDtoForUpdate userDto,
			HttpContext context)
		{
			await ControlConflictForUserAsync(
				userDto.Email,
				userDto.PhoneNumber);

			#region  get user by id (throw)
			var user = await _userManager.FindByEmailAsync(email);

			// when user not found
			if (user == null)
				throw new Exception("User not found.");
			#endregion

			#region update user
			user.NameSurname = userDto.NameSurname ?? user.NameSurname;
			#region phone
			if (userDto.PhoneNumber != null)
			{
				var tokenForChangePhone = await _userManager
					.GenerateChangePhoneNumberTokenAsync(user, userDto.PhoneNumber);

				await _userManager.ChangePhoneNumberAsync(
					user,
					userDto.PhoneNumber,
					tokenForChangePhone);
			}
			#endregion
			#region email
			if (userDto.Email != null)
			{
				#region update email
				var tokenForChangeEmail = await _userManager
					.GenerateChangeEmailTokenAsync(user, userDto.Email);

				await _userManager.ChangeEmailAsync(
					user,
					userDto.Email,
					tokenForChangeEmail);
				#endregion

				#region update fields related email
				user.UserName = userDto.Email;
				user.NormalizedEmail = userDto.Email.ToUpper();
				user.NormalizedUserName = userDto.Email.ToUpper();
				#endregion
			}
			#endregion
			user.Flag = userDto.Flag ?? user.Flag;
			user.NewPassportNo = userDto.NewPassportNo ?? user.NewPassportNo;
			user.OldPassportNo = userDto.OldPassportNo ?? user.OldPassportNo;
			user.Rank = userDto.Rank ?? user.Rank;
			user.DateOfIssue = userDto.DateOfIssue ?? user.DateOfIssue;
			user.PassPortExpiry = userDto.PassPortExpiry ?? user.PassPortExpiry;
			user.Nationality = userDto.Nationality ?? user.Nationality;
			user.DateOfBirth = userDto.DateOfBirth ?? user.DateOfBirth;
			user.PlaceOfBirth = userDto.PlaceOfBirth ?? user.PlaceOfBirth;
			user.Gender = userDto.Gender ?? user.Gender;
			user.YacthType = userDto.YacthType ?? user.YacthType;
			user.YacthName = userDto.YacthName ?? user.YacthName;
			user.IsPersonel = userDto.IsPersonel ?? user.IsPersonel;
			#region password
			if (userDto.Password != null)
				await _userManager.ResetPasswordAsync(
					user,
					await _userManager.GeneratePasswordResetTokenAsync(user),
					userDto.Password);
			#endregion

			await _userManager.UpdateAsync(user);
			#endregion

			#region update role of user
			if (userDto.RoleName != null)
			{
				var sql = "EXEC Role_Update " +
					"@UserId = {0}, " +
					"@RoleId = {1}";

				await _userRepository.ExecuteSqlRawAsync(
					sql,
					user.Id,
					userDto.RoleName);
			}
			#endregion

			#region generate new token with new claims
			var newToken = await GenerateTokenForUserAsync(
				user,
				userDto.RoleName == null ?
					"null"
					: userDto.RoleName.ToString());
			#endregion

			return newToken;
		}

		public async Task DeleteUsersAsync(UserDtoForDelete userDto)
		{
			#region delete users by email
			foreach (var email in userDto.Emails)
			{
				var user = await _userManager.FindByEmailAsync(email);
				await _userManager.DeleteAsync(user);
			}
			#endregion
		}

		#endregion

		#region forgot password

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
				await SendOtp(model);
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
			var token = await VerifyOtp(loginParams.VerificationCode, user.Id);

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

		#endregion


		public async Task<object> LoginForMobileAsync(UserLoginDto userDto)
		{
			var token = await LoginAsync(userDto, Roles.User, Roles.Admin);

			return token;
		}

		public async Task LoginForPanelAsync(UserLoginDto userDto)
		{
			var token = LoginAsync(userDto, Roles.Admin);

		}
	}


	public partial class UserService  // By MERT (PRIVATE)
	{
		private readonly JwtSettingsConfig _jwtSettingsConfig;
		private readonly IRoleService _roleService;


		private async Task ControlConflictForUserAsync(
			string? email = null,
			string? phone = null)
		{
			#region control email or phone whether conflict (throw)
			if (email != null || phone != null)
			{
				#region get users that have same email or phone
				var conflictedUsers = _userRepository
					.Where(u => u.Email.Equals(email)
						|| u.PhoneNumber.Equals(phone))
					.ToList();
				#endregion

				#region when conflict occured (throw)
				if (conflictedUsers.Count != 0)
				{
					#region when phone is conflicted (throw)
					var conflictedUserByPhone = conflictedUsers
						.Where(u => u.PhoneNumber.Equals(phone))
						.ToList();

					if (conflictedUserByPhone.Count != 0)
						throw new MiarException(
							409,
							"CE-U-P",
							"Conflict Error - User - Phone",
							"girilen telefon numarası zaten kayıtlı"
						);
					#endregion

					#region when email is conflicted (throw)
					var conflictedUserByEmail = conflictedUsers
						.Where(u => u.Email.Equals(email))
						.ToList();

					if (conflictedUserByEmail.Count != 0)
						throw new MiarException(
							409,
							"CE-U-E",
							"Conflict Error - User - Email",
							"girilen email zaten kayıtlı"
						);
					#endregion
				}
				#endregion
			}
			#endregion
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
	}
}