using MediatR;
using SmartFactorySample.DataReception.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Entities.TagInfo.Queries.GetTagInfo
{
    public class GetTagInfoQuery : IRequest<TagFullInfoDto>
    {
        public int Id { get; set; }
    }
}
