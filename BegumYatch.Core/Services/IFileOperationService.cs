using BegumYatch.Core.Models.FileOperations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IFileOperationService
    {
   
        Task DownloadFile(int Id);

        Task UploadFile(int entityType, int entityId, List<IFormFile> fileData);

        Task<List<string>> GetFilesById(int entityType, int entityId);
    }
}
