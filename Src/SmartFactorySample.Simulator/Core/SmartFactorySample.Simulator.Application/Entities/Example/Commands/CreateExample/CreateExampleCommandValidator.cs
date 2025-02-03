using FluentValidation;
using SmartFactorySample.Simulator.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Entities.Example.Commands.CreateExample
{
    public class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
    {
        private readonly IExampleManager _context;

        public CreateExampleCommandValidator(IExampleManager context)
        {
            _context = context;

            RuleFor(v => v.Value)
                .NotEmpty().WithMessage("Value is required.")
                .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");
            RuleFor(v => v.Id)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
        }
    }
}
