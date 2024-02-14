using BegumYatch.Core.DTOs.UserRegister;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Validations.User
{
    public class CrewAndPassengerUpdateValidator : AbstractValidator<CrewAndPassengerUpdateDto>
    {
        public CrewAndPassengerUpdateValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.NameSurname).MaximumLength(100).WithMessage("{PropertyName} must be a maximum of 100 characters.");
            RuleFor(x => x.NewPassportNo).Length(7).WithMessage("{PropertyName} must be a 7 characters.");
            RuleFor(x => x.OldPassportNo).Length(9).WithMessage("{PropertyName} must be a 9 characters.");
            //RuleFor(x => x.Rank).MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters.");
            RuleFor(p => p.DateOfIssue).Must(BeAValidDateofIssue).WithMessage("Invalid {PropertyName}");
            RuleFor(p => p.PassPortExpiry).Must(BeAValidPassPortExpiry).WithMessage("Invalid {PropertyName}");
            RuleFor(x => x.PlaceOfBirth).MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters.");
        }
        //pasaport verilme tarihi şuandan küçük olmalıdır ve büyük olmamalıdır.
        protected bool BeAValidDateofIssue(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            int dateOfIssue = date.Year;

            if (dateOfIssue < currentYear)
                return true;


            return false;
        }

        protected bool BeAValidPassPortExpiry(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            int passPortExpiry = date.Year;

            if (passPortExpiry > currentYear)
                return true;


            return false;
        }
    }
}
