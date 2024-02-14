using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.Role
{
    public class RoleUpdateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Rol ismi boş bırakılamaz.")]
        public string Name { get; set; }
    }
}
