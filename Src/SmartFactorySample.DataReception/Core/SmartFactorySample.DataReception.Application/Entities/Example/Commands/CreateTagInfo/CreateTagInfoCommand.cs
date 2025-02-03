using MediatR;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.CreateTagInfo
{
    public class CreateTagInfoCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateTagInfoCommandHandler : IRequestHandler<CreateTagInfoCommand, bool>
    {
        private readonly ITagInfoManager _context;

        public CreateTagInfoCommandHandler(ITagInfoManager context)
        {
            _context = context;
        }

        public Task<bool> Handle(CreateTagInfoCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.CreateTagInfo(request));
        }
    }
}
