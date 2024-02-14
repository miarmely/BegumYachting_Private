using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IUserService : IService<AppUser>
    {
        Task<GetPersonelInfoByIdDto> GetPersonelInfo(int id);
        Task<List<ReturnResponseModel>> IsChangeEmail(SendConfirmCodeUpdateEmail sendConfirmCodeUpdateEmailDto);
        Task<List<ReturnResponseModel>> VerifyConfirmCode(VerifyConfirmCode verifyConfirmCode);
        Task<String> SendOtp(MailOtp model);
        Task<String> VerifyOtp(String code, int UserId);
        Task<List<GetUsersDto>> GetAllUsers();
        Task<int> UpdateCrewAndPassenger(CrewAndPassengerUpdateDto crewAndPassengerUpdateDto);
      //  Task<IDataResult<UsersDto>> CreateUser(UserAddViewModel userAddViewModel);
        //bunun updateni eklemeyi unutma
    }
}
