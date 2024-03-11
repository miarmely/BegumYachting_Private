﻿using AutoMapper;
using BegumYatch.Core.Configs;
using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Core.ViewModels;
using BegumYatch.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

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
        private readonly JwtSettingsConfig _jwtSettingsConfig;

        public UserService(IGenericRepository<AppUser> userRepository, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<AppUser> userManager, IGenericRepository<MailOtp> mailOtpRepository, 
             IOptions<JwtSettingsConfig> jwtSettingsConfig) 
            : base(userRepository, unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
            _mailOtpRepository = mailOtpRepository;
            _jwtSettingsConfig = jwtSettingsConfig.Value;
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
    }

    public partial class UserService  // By MERT
    {
        public async Task<object> LoginAsync(UserLoginDto userDto)
        {
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

            return new
            {
                user.Id,
                Token = await GenerateTokenForUserAsync(user)
            };
        }

        public async Task CreateUserAsync(UserDtoForCreate userDto)
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

            #region when any error occured (error)
            if (!result.Succeeded)
                throw new MiarException(
                    500,
                    "ISE",
                    "Internal Server Error",
                    "kullanıcı eklenirken bir hata oluştu");
            #endregion
        }

        public async Task<List<GetUsersDto>> MiarGetAllUsers(int accountId)
        {
            #region get users except account owner (THROW)
            var users = await _userRepository
                .GetAll()
                .Where(u => u.Id != accountId)
                .AsNoTracking()
                .ToListAsync();

            // when any user not found
            if (users.Count == 0)
                throw new MiarException(
                    404,
                    "NF-U",
                    "Not Found - User",
                    "kullanıcı bulunamadı");

            var usersDtoList = _mapper.Map<List<GetUsersDto>>(users);
            #endregion

            return usersDtoList;
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

        public async Task UpdateUserAsync(string email, UserDtoForUpdate userDto)
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
            //await _unitOfWork.CommitAsync();
            #endregion
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
    }

    public partial class UserService  // By MERT (PRIVATE)
    {
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

        private async Task<string> GenerateTokenForUserAsync(AppUser user)
        {
            #region set claims
            var claims = new Collection<Claim>
            {
                new ("Id", user.Id.ToString()),
                //new ("NameSurname", user.NameSurname),
                //new ("PhoneNumber", user.PhoneNumber),
                //new ("Email", user.Email),
                //new ("Gender", user.Gender),
                //new ("Nationality", user.Nationality),
                //new ("YachtType", user.YacthType.ToString()),
                //new ("YachtName", user.YacthName),
                //new ("Flag", user.Flag),
                //new ("NewPassportNo", user.NewPassportNo),
                //new ("OldPassportNo", user.OldPassportNo),
                //new ("Rank", user.Rank),
                //new ("DateOfIssue", user.DateOfIssue.ToString()),
                //new ("PassPortExpiry", user.PassPortExpiry.ToString()),
                //new ("DateOfBirth", user.DateOfBirth.ToString()),
                //new ("PlaceOfBirth", user.PlaceOfBirth),
                //new ("IsPersonel", user.IsPersonel.ToString()),
                //new ("IsDeleted", user.IsDeleted.ToString()),
            };
            #endregion

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
                claims,
                signingCredentials: signingCredentials);
            #endregion
            
            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}