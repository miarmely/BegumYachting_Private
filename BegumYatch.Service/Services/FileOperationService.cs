using BegumYatch.Core.DTOs.FileOperations;
using BegumYatch.Core.Models.FileOperations;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Repository;
using BegumYatch.Service.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class FileOperationService : IFileOperationService
    {
        private readonly AppDbContext appDbContext;
        private readonly IFileOperationRepository _fileOperationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public FileOperationService(AppDbContext appDbContext, IFileOperationRepository fileOperationRepository, IUnitOfWork unitOfWork)
        {
            this.appDbContext = appDbContext;
            _fileOperationRepository = fileOperationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task UploadFile(int entityType, int entityId, List<IFormFile> fileData)
        {
            try
            {
                foreach (var file in fileData)
                {

                    var fileDetails = new FileDetail()
                    {
                        Id = 0,
                        FileName = file.FileName,
                        EntityId = entityId,
                        EntityType = entityType

                    };
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }
                    var result= _fileOperationRepository.AddAsync(fileDetails);
                   // var result2 = appDbContext.FileDetails.Add(fileDetails);
                }
                await _unitOfWork.CommitAsync();
               // await appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DownloadFile(int Id)
        {
            try
            {
                var file =_fileOperationRepository.Where(x => x.Id == Id).FirstOrDefaultAsync();

                var content = new System.IO.MemoryStream(file.Result.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "Uploads\\StaticContent\\",
                   file.Result.FileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        public async Task<List<string>> GetFilesById(int entityType, int entityId)
        {
            var relationFiles = await _fileOperationRepository.Where(x => x.EntityType == entityType && x.EntityId == entityId).ToListAsync();
            if (relationFiles.Count == 0)
                return null;
            else
                return relationFiles.Select(x => x.FileName).ToList();
        }

       
    }
}
