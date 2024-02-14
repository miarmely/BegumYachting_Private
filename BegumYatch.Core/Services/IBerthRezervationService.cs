using BegumYatch.Core.DTOs.BerthRezervation;
using BegumYatch.Core.Models.BerthRezervation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IBerthRezervationService : IService<BerthRezervation>
    {
        Task AddBerthRezervation(AddBerthRezervationDto addBerthRezervation);
    }
}
