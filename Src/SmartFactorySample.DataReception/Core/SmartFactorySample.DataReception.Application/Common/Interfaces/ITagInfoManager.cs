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
        public bool CreateTagInfo(CreateTagInfoCommand request);
        public bool UpdateTagInfo(UpdateTagInfoCommand request);
        public bool DeleteTagInfo(DeleteTagInfoCommand request);
        public TagFullInfoDto GetTagInfo(GetTagInfoQuery request);
    }
}
