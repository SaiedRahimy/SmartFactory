using SmartFactorySample.DataReception.Application.Common.Exceptions;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
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

namespace SmartFactorySample.DataReception.Infrastructure.Persistence.Example
{
    public class ExampleManager : ITagInfoManager
    {
        private readonly Dictionary<int, string> keyValues;

        public ExampleManager()
        {
            keyValues = new Dictionary<int, string>();
        }

        public bool CreateTagInfo(CreateTagInfoCommand request)
        {
            // add log
            keyValues.Add(request.Id, request.Name);
            return true;
        }



        public bool DeleteTagInfo(DeleteTagInfoCommand request)
        {
            // add log
            CheckKeyExist(request.Id);
            var result = keyValues.Remove(request.Id);
            return result;
        }



        public TagFullInfoDto GetTagInfo(GetTagInfoQuery request)
        {
            // add log
            var result = keyValues.TryGetValue(request.Id, out string Name);
            if (result)
            {
                return new TagFullInfoDto()
                {
                    Id = request.Id,
                    Name = Name
                };
            }
            else
                throw new NotFoundException($"Could not find value with id {request.Id}.");
        }



        public bool UpdateTagInfo(UpdateTagInfoCommand request)
        {
            // add log
            CheckKeyExist(request.Id);
            keyValues[request.Id] = request.Value;
            return true;
        }



        private void CheckKeyExist(int key)
        {
            if (!keyValues.ContainsKey(key))
            {
                throw new NotFoundException($"Could not find value with id {key}.");
            }
        }
    }
}
