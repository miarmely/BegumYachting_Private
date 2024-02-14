using BegumYatch.Core.DTOs.UserLogin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Validations.User
{
    public class UserLoginValidator:AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("{PropertyName} is required").EmailAddress().WithMessage("A valid email is required"); 
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
