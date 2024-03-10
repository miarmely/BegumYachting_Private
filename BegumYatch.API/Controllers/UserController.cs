﻿using AutoMapper;
using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.DTOs.Password;
using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Service.Helper;
using BegumYatch.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BegumYatch.API.Controllers
{
    public partial class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IMailDtoRepository _emailRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, IEmailService emailService, IUserService userService, IUnitOfWork unitOfWork, IMailDtoRepository emailRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _emailRepo = emailRepo;
        }


        [HttpPost]
        [Route("UserRegister")]
        public async Task<IActionResult> UserRegister(
            [FromBody] UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            else
            {
                var mapUser = _mapper.Map<AppUser>(userRegisterDto);
                mapUser.UserName = userRegisterDto.Email/*.Split('@')[0]*/;
                mapUser.DateOfIssue = DateTime.MinValue; // temporary
                mapUser.PassPortExpiry = DateTime.MinValue;  // temporary
                mapUser.DateOfBirth = DateTime.MinValue;  // temporary

                var result = await _userManager.CreateAsync(
                    mapUser,
                    userRegisterDto.Password);

                if (result.Succeeded)
                    return Ok(result);

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
        }


        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin(
            [FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                #region control user whether exists (By MERT)
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

                if (user == null)
                    return Unauthorized(userLoginDto);
                #endregion

                var userrole = "AdvancedRole";
                var result = await _signInManager.PasswordSignInAsync(userLoginDto.Email, userLoginDto.Password, false, false);

                if (!result.Succeeded)
                    return Unauthorized(userLoginDto);

                //create the current user claims principal
                var claimsPrincipal = await _signInManager
                    .CreateUserPrincipalAsync(user);
                //get the current user's claims.
                var claimresult = claimsPrincipal.Claims.ToList();
                //if it doesn't contains the Role claims, add a role claims
                if (!claimresult.Any(c => c.Type == ClaimTypes.Role))
                {
                    //add claims to current user. 
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userrole));
                }
                //refresh the Login
                await _signInManager.RefreshSignInAsync(user);

                var userInfo = new
                {
                    succeeded = result.Succeeded,
                    isLockedOut = result.IsLockedOut,
                    isNotAllowed = result.IsNotAllowed,
                    requiresTwoFactor = result.RequiresTwoFactor,
                    userId = user.Id,
                };

                return Ok(userInfo);
            }
        }


        [HttpGet]
        [Route("GetPersonelInfo")]
        public async Task<IActionResult> GetPersonelInfo(int id)
        {
            var personelInfo = await _userService.GetPersonelInfo(id);
            if (personelInfo == null)
            {
                throw new Exception("Böyle bir kullanıcı bulunamamıştır");
            }
            return Ok(personelInfo);
        }


        [HttpPut]
        [Route("VerifyConfirmCode")]
        // [Authorize(Policy = "Permissions.AllEntity.ReadCreateUpdate")]
        public async Task<IActionResult> VerifyConfirmCode([FromBody] VerifyConfirmCode verifyConfirmCode)
        {
            var response = await _userService.VerifyConfirmCode(verifyConfirmCode);
            return Ok(response);
        }


        [HttpPost]
        [Route("IsChangeEmail")]
        // [Authorize(Policy = "Permissions.AllEntity.ReadCreateUpdate")]
        public async Task<IActionResult> IsChangeEmail([FromBody] SendConfirmCodeUpdateEmail sendConfirmCodeUpdateEmailDto)
        {
            var response = await _userService.IsChangeEmail(sendConfirmCodeUpdateEmailDto);
            return Ok(response);
        }


        [HttpGet]
        [Route("GetAllUsers")]
        // [Authorize(Policy = "Permissions.AllEntity.ReadCreateUpdate")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers<GetUsersDto>();

            return Ok(response);
        }


        [HttpDelete]
        [Route("UserDelete")]
        public async Task<IActionResult> UserDelete(int id)
        {
            var user = await _userService.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound("Bu id'ye ait kullanıcı bulunamadı");
            await _userManager.DeleteAsync(user);
            await _unitOfWork.CommitAsync();
            return Ok(user.Id);
        }


        [HttpPost]
        [Route("UserLogOut")]
        [Authorize]
        public async Task<IActionResult> UserLogOut()
        {
            //çıkış yaptıktan sonra nereye return olacağını frontend mi belirleyecek ? (By RÜMEYSA)
            await _signInManager.SignOutAsync();
            return Ok();
        }


        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var hasUser = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır.");
                return BadRequest(ModelState);
            }

            var chars = "0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            string passwordResestToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            //var passwordResetLink = Url.Action("ResetPassword", "User", new { userId = hasUser.Id, Token = passwordResestToken }, HttpContext.Request.Scheme);

            var model = new MailOtp
            {
                UserId = hasUser.Id,
                Email = hasUser.Email,
                Code = result,
                DateAdded = DateTime.Now,
                Token = passwordResestToken
            };

            try
            {
                await _userService.SendOtp(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }




            await _emailService.SendEmailAsync("Forget Password", result, hasUser.Email!);

            return Ok();
        }


        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var userId = request.UserId;


            if (userId == 0)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Kullanıcı bulunamamıştır.");
                return BadRequest(ModelState);
            }
            var response = await _userService.VerifyOtp(request.Code, request.UserId);
            if (response != "")
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(hasUser, response, request.Password);
                return Ok(result);
            }
            else
            {
                throw new Exception("Kod Yanlış!");

            }
        }
    }

    public partial class UserController // By MERT
    {
        [HttpPost("adminPanel/login")]
        public async Task<IActionResult> Login(
            [FromBody] UserLoginDto userDto)
        {
            var responseObject = await _userService.LoginAsync(userDto);

            return Ok(responseObject);
        }


        [HttpPost("adminPanel/userCreate")]
        public async Task<IActionResult> CreateUser(
            [FromBody] UserDtoForCreate userDto)
        {
            await _userService.CreateUserAsync(userDto);

            return NoContent();
        }


        [HttpGet("adminPanel/userDisplay/Id")]
        public async Task<IActionResult> GetUserInfos(
            [FromQuery] UserParamsForDisplayById userParams)
        {
            var users = await _userService
                .GetUsersByFilteringAsync(
                    userParams.UserId,
                    CheckIsDeleted: userParams.CheckIsDeleted);

            return Ok(users
                .FirstOrDefault());
        }


        [HttpGet("adminPanel/userDisplay/filter")]
        public async Task<IActionResult> GetUserInfos(
            [FromQuery] UserParamsForDisplayByFiltering userParams)
        {
            var users = await _userService.GetUsersByFilteringAsync(
                Email: userParams.Email,
                Phone: userParams.Phone,
                CheckIsDeleted: userParams.CheckIsDeleted);

            return Ok(users);
        }


        [HttpPost("adminPanel/userUpdate")]
        public async Task<IActionResult> UpdateUser(
            [FromQuery(Name = "email")] string email,
            [FromBody] UserDtoForUpdate userDto)
        {
            await _userService.UpdateUserAsync(email, userDto);

            return NoContent();
        }


        [HttpPost("adminPanel/userDelete")]
        public async Task<IActionResult> DeleteUsers(
            [FromBody] UserDtoForDelete userDto)
        {
            await _userService.DeleteUsersAsync(userDto);

            return NoContent();
        }


        //[HttpGet("adminPanel/getAllUsers/paging")]
        //public async Task<IActionResult> GetAllUsers(
        //    [FromQuery(Name = "start")] int start = 1,
        //    [FromQuery(Name = "length")] int length = 10)
        //{
        //    #region get users as pagined
        //    //var offset = (pageNumber - 1) * pageSize;
        //    var allUsers = await _userService.GetAllUsers<GetUsersDto>();
        //    var paginedUsers = allUsers
        //        .Skip(start - 1)
        //        .Take(length);
        //    #endregion

        //    return Ok(new
        //    {
        //        Data = paginedUsers,
        //        RecordsFiltered = paginedUsers.Count(),
        //        RecordsTotal = allUsers.Count,
        //        Draw = 1
        //    });
        //}
    }
}