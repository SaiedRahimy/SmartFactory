using FluentValidation;
using SmartFactorySample.Simulator.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Entities.Example.Commands.DeleteExample
{
    public class DeleteExampleCommandValidator : AbstractValidator<DeleteExampleCommand>
    {
        private readonly IExampleManager _context;

        public DeleteExampleCommandValidator(IExampleManager context)
        {
            _context = context;

            RuleFor(v => v.Id)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
        }
    }
}
