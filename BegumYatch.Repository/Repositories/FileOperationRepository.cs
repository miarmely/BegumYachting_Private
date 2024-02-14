using BegumYatch.Core.Models.FileOperations;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    internal class FileOperationRepository : GenericRepository<FileDetail>, IFileOperationRepository
    {
        public FileOperationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
