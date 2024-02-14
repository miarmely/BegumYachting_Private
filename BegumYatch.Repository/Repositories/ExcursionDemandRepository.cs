using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    public class ExcursionDemandRepository : GenericRepository<ExcursionDemand>, IExcursionDemandRepository
    {
        public ExcursionDemandRepository(AppDbContext context) : base(context)
        {
        }
    }
}
