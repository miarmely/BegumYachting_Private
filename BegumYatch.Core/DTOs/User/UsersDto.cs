using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.User
{
    public class UsersDto : DtoBase
    {
        public AppUser User { get; set; }
    }
}
