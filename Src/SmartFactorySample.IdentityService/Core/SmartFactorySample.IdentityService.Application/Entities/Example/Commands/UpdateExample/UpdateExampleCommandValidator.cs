using FluentValidation;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Application.Entities.Example.Commands.UpdateExample
{
    public class UpdateExampleCommandValidator : AbstractValidator<UpdateExampleCommand>
    {
        private readonly IExampleManager _context;

        public UpdateExampleCommandValidator(IExampleManager context)
        {
            _context = context;

            RuleFor(v => v.Id)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
            RuleFor(v => v.Value)
                .NotEmpty().WithMessage("Value is required.")
                .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");
        }
    }
}
