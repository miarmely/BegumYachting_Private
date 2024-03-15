﻿using BegumYatch.Core.Models.AdminPanel;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;


namespace BegumYatch.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Temp> _repository;

        public RoleService(IGenericRepository<Temp> repository)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<string>> GetAllRoleNamesAsync()
        {
            #region get role names
            var roles = await _repository
                .FromSqlRawAsync<MiarRole>("SELECT * FROM Roles");

            var roleNames = roles.Select(r => r.RoleName);
            #endregion

            return roleNames;
        }
    }
}
