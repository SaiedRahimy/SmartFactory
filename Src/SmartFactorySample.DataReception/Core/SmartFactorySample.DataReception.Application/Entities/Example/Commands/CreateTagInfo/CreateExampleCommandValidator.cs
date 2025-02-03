using FluentValidation;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.CreateTagInfo
{
    public class CreateExampleCommandValidator : AbstractValidator<CreateTagInfoCommand>
    {
        private readonly ITagInfoManager _context;

        public CreateExampleCommandValidator(ITagInfoManager context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Value is required.")
                .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");
            RuleFor(v => v.Id)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
        }
    }
}
