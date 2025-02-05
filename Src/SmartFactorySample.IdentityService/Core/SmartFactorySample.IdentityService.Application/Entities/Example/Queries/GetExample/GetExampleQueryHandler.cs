using MediatR;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using SmartFactorySample.IdentityService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Application.Entities.Example.Queries.GetExample
{
    public class GetExampleQueryHandler : IRequestHandler<GetExampleQuery, ExampleDto>
    {
        private readonly IExampleManager _context;

        public GetExampleQueryHandler(IExampleManager context)
        {
            _context = context;
        }

        public Task<ExampleDto> Handle(GetExampleQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.GetExample(request));
        }
    }
}
