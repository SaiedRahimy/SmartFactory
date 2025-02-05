
using MediatR;
using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using SmartFactorySample.DataPresentation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Application.Entities.TagInfo.Commands
{
    public class RegisterTagInfosCommand : IRequest<List<TagInfoDto>>
    {
        public List<int> TagIds { get; set; }
    }

    public class RegisterTagInfosCommandHandler : IRequestHandler<RegisterTagInfosCommand, List<TagInfoDto>>
    {
        private readonly ICacheService _cacheService;

        public RegisterTagInfosCommandHandler(ICacheService cacheServic)
        {
            _cacheService = cacheServic;
        }

        public Task<List<TagInfoDto>> Handle(RegisterTagInfosCommand request, CancellationToken cancellationToken)
        {
            var result= _cacheService.GetData(request.TagIds);

            return Task.FromResult(result);
        }
    }
}
