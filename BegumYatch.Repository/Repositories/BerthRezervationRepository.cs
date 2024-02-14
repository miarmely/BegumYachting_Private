using BegumYatch.Core.Models.BerthRezervation;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    public class BerthRezervationRepository : GenericRepository<BerthRezervation>, IBerthRezervationRepository
    {
        public BerthRezervationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
