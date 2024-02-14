using BegumYatch.Core.Models.Orders;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    public class ProvisionOrderRepository : GenericRepository<ProvisionOrder>, IProvisionOrderRepository
    {
        public ProvisionOrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
