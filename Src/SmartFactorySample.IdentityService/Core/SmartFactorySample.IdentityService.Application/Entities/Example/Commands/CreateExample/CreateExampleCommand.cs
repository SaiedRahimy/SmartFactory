using MediatR;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Application.Entities.Example.Commands.CreateExample
{
    public class CreateExampleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class CreateExampleCommandHandler : IRequestHandler<CreateExampleCommand, bool>
    {
        private readonly IExampleManager _context;

        public CreateExampleCommandHandler(IExampleManager context)
        {
            _context = context;
        }

        public Task<bool> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.CreateExample(request));
        }
    }
}
