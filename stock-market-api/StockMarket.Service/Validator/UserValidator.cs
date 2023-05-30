using FluentValidation;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Validator
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage("Name is Required");

            RuleFor(i => i.Email)
                .EmailAddress()
                .WithMessage("Invalid Email")
                .NotEmpty()
                .WithMessage("Email is required");

            RuleFor(i => i.Password)
                .NotEmpty()
                .WithMessage("Password is required");            
        }
    }
}
