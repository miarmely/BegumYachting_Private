using BegumYatch.Core.Models.Orders;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    public class FlowerOrderRepository : GenericRepository<FlowerOrder>, IFlowerOrderRepository
    {
        public FlowerOrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
