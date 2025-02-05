using MediatR;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Application.Entities.Example.Commands.UpdateExample
{
    public class UpdateExampleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class UpdateExampleCommandHandler : IRequestHandler<UpdateExampleCommand, bool>
    {
        private readonly IExampleManager _context;

        public UpdateExampleCommandHandler(IExampleManager context)
        {
            _context = context;
        }

        public Task<bool> Handle(UpdateExampleCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.UpdateExample(request));
        }
    }
}
