using MediatR;
using SmartFactorySample.Simulator.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Entities.Example.Commands.DeleteExample
{
    public class DeleteExampleCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteExampleCommandHandler : IRequestHandler<DeleteExampleCommand, bool>
    {
        private readonly IExampleManager _context;

        public DeleteExampleCommandHandler(IExampleManager context)
        {
            _context = context;
        }

        public Task<bool> Handle(DeleteExampleCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.DeleteExample(request));
        }
    }
}
