using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.FileOperations
{
    public class FilesDto
    {
        public List<IFormFile> Notes { get; set; }
    }
}
