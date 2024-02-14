using BegumYatch.Core.DTOs.UserRegister;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Validations.User
{
    public class AddPersonelInfoValidator : AbstractValidator<PersonelInfoDto>
    {
        public AddPersonelInfoValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("{PropertyName} is required").GreaterThan(0);
            RuleFor(x => x.YacthName).NotNull().NotEmpty().WithMessage("{PropertyName} is required"); 
            RuleFor(x => x.YacthType).NotNull().NotEmpty().WithMessage("{PropertyName} is required"); 
            RuleFor(x => x.Flag).NotNull().NotEmpty().WithMessage("{PropertyName} is required"); 
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
