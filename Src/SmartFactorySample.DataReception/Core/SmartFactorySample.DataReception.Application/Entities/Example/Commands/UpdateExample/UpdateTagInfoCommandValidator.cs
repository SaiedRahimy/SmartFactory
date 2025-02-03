using FluentValidation;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.UpdateTagInfo
{
    public class UpdateTagInfoCommandValidator : AbstractValidator<UpdateTagInfoCommand>
    {
        private readonly ITagInfoManager _context;

        public UpdateTagInfoCommandValidator(ITagInfoManager context)
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
