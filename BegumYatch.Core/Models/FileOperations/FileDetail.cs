using BegumYatch.Core.Models.Demands;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.FileOperations
{
    public class FileDetail:BaseEntity
    {       
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public int EntityType { get; set; }
        public int EntityId { get; set; }
    }
}
