using BegumYatch.Core.Models.Orders;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    public class TechnicalAssitanceandSparePartOrderRepository : GenericRepository<TechnicalAssitanceandSparePartOrder>, ITechnicalAssitanceandSparePartOrderRepository
    {
        public TechnicalAssitanceandSparePartOrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
