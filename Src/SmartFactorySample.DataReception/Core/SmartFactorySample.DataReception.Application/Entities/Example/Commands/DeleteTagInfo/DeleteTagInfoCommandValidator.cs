using FluentValidation;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.DeleteTagInfo
{
    public class DeleteTagInfoCommandValidator : AbstractValidator<DeleteTagInfoCommand>
    {
        private readonly ITagInfoManager _context;

        public DeleteTagInfoCommandValidator(ITagInfoManager context)
        {
            _context = context;

            RuleFor(v => v.Id)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
        }
    }
}
