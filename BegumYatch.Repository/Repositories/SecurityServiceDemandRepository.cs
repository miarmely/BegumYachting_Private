using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    public class SecurityServiceDemandRepository : GenericRepository<SecurityServiceDemand>, ISecurityServiceDemandRepository
    {
        public SecurityServiceDemandRepository(AppDbContext context) : base(context)
        {
        }
    }
}
