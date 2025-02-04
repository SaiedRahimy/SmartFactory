using SmartFactorySample.DataReception.Application.Dtos;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.CreateTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.DeleteTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.UpdateTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Queries.GetTagInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Common.Interfaces
{
    public interface ITagInfoManager
    {
        Task<bool> CreateTagInfo(CreateTagInfoCommand request);
        Task<bool> UpdateTagInfo(UpdateTagInfoCommand request);
        Task<bool> DeleteTagInfo(DeleteTagInfoCommand request);
        Task<TagFullInfoDto> GetTagInfo(GetTagInfoQuery request);

        Task RegisterNewTags(List<TagInfoDto> baseSensorDataDtos);
        void EnrichTagId(TagInfoDto tag);
    }
}
