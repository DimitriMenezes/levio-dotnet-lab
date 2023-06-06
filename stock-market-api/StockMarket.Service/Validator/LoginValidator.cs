using FluentValidation;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Validator
{
    public class LoginValidator : AbstractValidator<LoginModel> 
    {
        public LoginValidator()
        {
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
