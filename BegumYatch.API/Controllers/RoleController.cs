using AutoMapper;
using BegumYatch.API.Filters.AdminPanel.Attributes;
using BegumYatch.Core.DTOs.Role;
using BegumYatch.Core.DTOs.RoleCreate;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BegumYatch.API.Controllers
{
   // [Authorize]
    public partial class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IRoleService roleService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _roleService = roleService;
        }

        [HttpGet]
        [Route("RolesList")]
        [Authorize(Policy = "Permissions.AllEntity.Read")]
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.Select(x => new RolesListDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            var mapRoles = _mapper.Map<List<AppRole>>(roles);
            return Ok(mapRoles);

            //var userClaimList = User.Claims.Select(x => new ClaimViewModel()
            //{
            //    Issuer = x.Issuer,
            //    Type = x.Type,
            //    Value = x.Value
            //}).ToList();
            //return Ok(userClaimList);

        }

        [HttpPost]
        [Route("RoleCreate")]
        [Authorize(Policy = "Permissions.AllEntity.ReadCreateUpdate")]
        public async Task<IActionResult> RoleCreate(RoleCreateDto roleCreateDto)
        {
            var result = await _roleManager.CreateAsync(_mapper.Map<AppRole>(new AppRole() { Name = roleCreateDto.Name }));
            if (result.Succeeded)
                return Ok(result);
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("RoleUpdate")]

        public async Task<IActionResult> RoleUpdate(RoleUpdateDto roleUpdateDto)
        {
            var roleToUpdate = _roleManager.Roles.Where(x => x.Id == roleUpdateDto.Id).FirstOrDefault();
            if (roleToUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamıştır");
            }
            roleToUpdate.Name = roleUpdateDto.Name;
            var mapRoleToUpdate = _mapper.Map<AppRole>(roleToUpdate);
            await _roleManager.UpdateAsync(mapRoleToUpdate);
            return Ok(mapRoleToUpdate);

        }

        [HttpDelete]
        [Route("RoleDelete")]
        [Authorize/*(Policy = "Permissions.AllEntity.ReadCreateUpdateDelete")*/]
        public async Task<IActionResult> RoleDelete(int id)
        {
            var roleToDelete = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();
            if (roleToDelete == null)
            {
                throw new Exception("Silinecek rol bulunamamıştır");
            }
            var result = await _roleManager.DeleteAsync(roleToDelete);
            if (!result.Succeeded)
                throw new Exception(result.Errors.Select(x => x.Description).First());

            return Ok(result);
        }

        [HttpPost]
        [Route("RoleAssign")]
        
        public async Task<IActionResult> AssingRoleToUser(int userId, List<string> RoleName)
        {
            var currentUser = (_userManager.Users.Where(x => x.Id == userId).FirstOrDefault())!;
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            foreach (var role in roles)
            {
                if (RoleName.Contains(role.Name))
                    await _userManager.AddToRoleAsync(currentUser, role.Name);
                else
                    await _userManager.RemoveFromRoleAsync(currentUser, role.Name);
            }
            return Ok();
        }
    }


    public partial class RoleController  // By MERT
    {
        [HttpGet("adminPanel/roleDisplay")]
		[MiarApiAuthorize("Admin")]
        public async Task<IActionResult> GetAllRoleNames()
        {
            var roles = await _roleService.GetAllRoleNamesAsync();

            return Ok(roles);
        }
    }
}
