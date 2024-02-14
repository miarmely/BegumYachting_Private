using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.FileOperations
{
    public class FileOperationDto
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public int EntityType { get; set; }
        public int EntityId { get; set; }
    }
}
