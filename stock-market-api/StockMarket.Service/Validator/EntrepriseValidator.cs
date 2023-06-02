using FluentValidation;
using StockMarket.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Service.Validator
{
    public class EntrepriseValidator : AbstractValidator<EntrepriseModel>
    {
        public EntrepriseValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage("Name is Required");

            RuleFor(i => i.Code)                
                .NotEmpty()
                .WithMessage("Entreprise Code is required");
        }
    }
}
