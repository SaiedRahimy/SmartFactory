using SmartFactorySample.IdentityService.Application.Common.Exceptions;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using SmartFactorySample.IdentityService.Application.Dtos;
using SmartFactorySample.IdentityService.Application.Entities.Example.Commands.CreateExample;
using SmartFactorySample.IdentityService.Application.Entities.Example.Commands.DeleteExample;
using SmartFactorySample.IdentityService.Application.Entities.Example.Commands.UpdateExample;
using SmartFactorySample.IdentityService.Application.Entities.Example.Queries.GetExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Infrastructure.Persistence.Example
{
    public class ExampleManager : IExampleManager
    {
        private readonly Dictionary<int, string> keyValues;

        public ExampleManager()
        {
            keyValues = new Dictionary<int, string>();
        }

        public bool CreateExample(CreateExampleCommand request)
        {
            // add log
            keyValues.Add(request.Id, request.Value);
            return true;
        }



        public bool DeleteExample(DeleteExampleCommand request)
        {
            // add log
            CheckKeyExist(request.Id);
            var result = keyValues.Remove(request.Id);
            return result;
        }



        public ExampleDto GetExample(GetExampleQuery request)
        {
            // add log
            var result = keyValues.TryGetValue(request.Id, out string value);
            if (result)
            {
                return new ExampleDto()
                {
                    Id = request.Id,
                    Value = value
                };
            }
            else
                throw new NotFoundException($"Could not find value with id {request.Id}.");
        }



        public bool UpdateExample(UpdateExampleCommand request)
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
