using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Validations.FuelPurchaseDemand
{
    public class FuelPurchaseDemandValidator:AbstractValidator<AddFuelPurchaseDemandDto>
    {
        public FuelPurchaseDemandValidator()
        {
            //RuleFor(x => x.YacthName).NotNull().WithMessage("{PropertyName} zorunludur.").NotEmpty().WithMessage("{PropertyName} zorunludur.");
        }
    }
}
