using MediatR;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.DeleteTagInfo
{
    public class DeleteTagInfoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteTagInfoCommandHandler : IRequestHandler<DeleteTagInfoCommand, bool>
    {
        private readonly ITagInfoManager _context;

        public DeleteTagInfoCommandHandler(ITagInfoManager context)
        {
            _context = context;
        }

        public Task<bool> Handle(DeleteTagInfoCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.DeleteTagInfo(request));
        }
    }
}
