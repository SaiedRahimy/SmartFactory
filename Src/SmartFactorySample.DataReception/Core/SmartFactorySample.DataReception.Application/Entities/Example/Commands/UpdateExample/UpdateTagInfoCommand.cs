using MediatR;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.UpdateTagInfo
{
    public class UpdateTagInfoCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class UpdateTagInfoCommandHandler : IRequestHandler<UpdateTagInfoCommand, bool>
    {
        private readonly ITagInfoManager _context;

        public UpdateTagInfoCommandHandler(ITagInfoManager context)
        {
            _context = context;
        }

        public Task<bool> Handle(UpdateTagInfoCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.UpdateTagInfo(request));
        }
    }
}
