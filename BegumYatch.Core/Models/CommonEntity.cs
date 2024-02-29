using BegumYatch.Core.Models.FileOperations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models
{
    public class CommonEntity: BaseEntity
    {
        [NotMapped]
        public List<IFormFile> Notes { get; set; }
        public bool Status { get; set; }
    }
}
