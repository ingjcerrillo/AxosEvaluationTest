using com.ToysAndGames.Server.WebAPI.DAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.ToysAndGames.Server.WebAPI.ModelValidation
{
    /// <summary>
    /// Defines custom validation rules for Product.
    /// </summary>
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            //Age must be between 0 and 100.
            RuleFor(x => x.AgeRestriction)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Age must be between 0 and 100.");

            //Price must be between $1 and $1000
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(1000)
                .WithMessage("Price must be between $1 and $1000.");

            //InMemoryDb will not validate relational rules like field's lenght so we add it manually.
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty().NotNull();

            RuleFor(x => x.Description).MaximumLength(100);

            RuleFor(x => x.Company).MaximumLength(50).NotEmpty().NotNull();
        }
    }
}
