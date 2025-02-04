using MediatR;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using SmartFactorySample.DataReception.Application.Dtos;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Queries.GetTagInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Queries.GetTagInfo
{
    public class GetTagInfoQueryHandler : IRequestHandler<GetTagInfoQuery, TagFullInfoDto>
    {
        private readonly ITagInfoManager _context;

        public GetTagInfoQueryHandler(ITagInfoManager context)
        {
            _context = context;
        }

        public async Task<TagFullInfoDto> Handle(GetTagInfoQuery request, CancellationToken cancellationToken)
        {
            return await _context.GetTagInfo(request);
        }
    }
}
