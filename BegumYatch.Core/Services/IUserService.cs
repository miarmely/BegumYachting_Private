﻿using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Models.User;


namespace BegumYatch.Core.Services
{
    public interface IUserService : IService<AppUser>
    {
        Task<GetPersonelInfoByIdDto> GetPersonelInfo(int id);
        Task<List<ReturnResponseModel>> IsChangeEmail(SendConfirmCodeUpdateEmail sendConfirmCodeUpdateEmailDto);
        Task<List<ReturnResponseModel>> VerifyConfirmCode(VerifyConfirmCode verifyConfirmCode);
        Task<String> SendOtp(MailOtp model);
        Task<String> VerifyOtp(String code, int UserId);
        Task<List<TDto>> GetAllUsers<TDto>();
        Task UpdateUserAsync(string email, UserDtoForUpdate userDto);
        Task<string> ComputeMd5Async(string value);
      //  Task<IDataResult<UsersDto>> CreateUser(UserAddViewModel userAddViewModel);
        //bunun updateni eklemeyi unutma
    }
}
