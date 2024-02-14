using BegumYatch.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.ViewModels
{
    public class UserAddAjaxViewModel
    {
        public UserAddViewModel UserAddViewModel { get; set; }
        public string UserAddPartial { get; set; }
        public UsersDto UserDto { get; set; }
    }
}
