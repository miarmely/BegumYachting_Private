using BegumYatch.Core.Models.BerthRezervation;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository.Repositories
{
    public class MailDtoRepository : GenericRepository<MailOtp>, IMailDtoRepository
    {
        public MailDtoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
