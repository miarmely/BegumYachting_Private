using AutoMapper;
using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Core.ViewModels;
using BegumYatch.Repository.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class UserService : Service<AppUser>, IUserService
    {
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IGenericRepository<MailOtp> _mailOtpRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<AppUser> userRepository, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<AppUser> userManager, IGenericRepository<MailOtp> mailOtpRepository) : base(userRepository, unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
            _mailOtpRepository = mailOtpRepository;
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

            var aaa = _mailOtpRepository.GetAll().Where(x => x.UserId == userId && x.Code == code).ToList().OrderByDescending(x => x.DateAdded);

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

        public async Task UpdateUserAsync(string email, UserDtoForUpdate userDto)
        {
            #region  get user by id (error)
            var user = _userRepository
                .Where(x => x.Email == email)
                .FirstOrDefault();

            // when user not found
            if (user == null)
                throw new Exception("User not found.");
            #endregion

            #region update user
            if (userDto.NameSurname != null) user.NameSurname = userDto.NameSurname;
            if (userDto.PhoneNumber != null) user.PhoneNumber = userDto.PhoneNumber;
            if (userDto.Email != null) user.Email = userDto.Email;
            if (userDto.Flag != null) user.Flag = userDto.Flag;
            if (userDto.NewPassportNo != null)
                user.NewPassportNo = userDto.NewPassportNo;
            if (userDto.OldPassportNo != null)
                user.OldPassportNo = userDto.OldPassportNo;
            if (userDto.Rank != null) user.Rank = userDto.Rank;
            if (userDto.DateOfIssue != null) user.DateOfIssue = userDto.DateOfIssue;
            if (userDto.PassPortExpiry != null)
                user.PassPortExpiry = userDto.PassPortExpiry;
            if (userDto.Nationality != null) user.Nationality = userDto.Nationality;
            if (userDto.DateOfBirth != null) user.DateOfBirth = userDto.DateOfBirth;
            if (userDto.PlaceOfBirth != null)
                user.PlaceOfBirth = userDto.PlaceOfBirth;
            if (userDto.Gender != null) user.Gender = userDto.Gender;
            if (userDto.YacthType != null) user.YacthType = userDto.YacthType;
            if (userDto.YacthName != null) user.YacthName = userDto.YacthName;
            if (userDto.IsPersonel != null) user.IsPersonel = userDto.IsPersonel;
            if (userDto.Password != null) 
                user.PasswordHash = await ComputeMd5Async(userDto.Password);

            await _unitOfWork.CommitAsync();
            #endregion
        }

        private async Task<string> ComputeMd5Async(string value)
        {
            using (var md5 = MD5.Create())
            {
                #region hash the value
                var hashedValueInBytes = md5.ComputeHash(Encoding
                    .UTF8
                    .GetBytes(value));

                var hashedValueInBase64Str = Convert
                    .ToBase64String(hashedValueInBytes);
                #endregion

                return hashedValueInBase64Str;
            }
        }

        // BELONG TO RUMEYSA (REMOVED SERVICES) (By MERT)
        //public async Task<int> UpdateCrewAndPassenger(
        //    CrewAndPassengerUpdateDto crewAndPassengerUpdateDto)
        //{
        //    var distanceMonth = crewAndPassengerUpdateDto.PassPortExpiry.Month - DateTime.Now.Month;

        //    var currentUser = _userRepository
        //        .Where(x => x.Id == crewAndPassengerUpdateDto.Id)
        //        .FirstOrDefault();

        //    if (currentUser == null)
        //        throw new Exception("There is no user with this information.");

        //    if (crewAndPassengerUpdateDto.IsPersonel)
        //    {
        //        currentUser.IsPersonel = true;
        //        currentUser.Rank = crewAndPassengerUpdateDto.Rank;
        //    }

        //    else
        //    {
        //        currentUser.IsPersonel = false;
        //        currentUser.Rank = null;
        //    }
        //    //pasaportun bitiş tarihi bilgilendirme maili, 6 ay / 180 gün önce atılması sağlanmalıdır.

        //    if (distanceMonth < 0)
        //        distanceMonth = (-1) * distanceMonth;

        //    if (distanceMonth == 6)
        //        await _emailService.SendEmailAsync("Information about passport expiry date", "Dear " + $"{currentUser.NameSurname}" + " , there are 6 months until the expiry date of your passport. For your information.", currentUser.Email);
        //    //yeni ve eski pasaport doluluğunu kontrol et. ve bunu yukarıdaki kontrol kısmına oturt

        //    _userRepository.Update(currentUser);
        //    await _unitOfWork.CommitAsync();

        //    return crewAndPassengerUpdateDto.Id;
        //}
    }
}