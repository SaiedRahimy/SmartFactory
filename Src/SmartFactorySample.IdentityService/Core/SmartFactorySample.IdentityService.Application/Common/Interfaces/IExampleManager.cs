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

namespace SmartFactorySample.IdentityService.Application.Common.Interfaces
{
    public interface IExampleManager
    {
        public bool CreateExample(CreateExampleCommand request);
        public bool UpdateExample(UpdateExampleCommand request);
        public bool DeleteExample(DeleteExampleCommand request);
        public ExampleDto GetExample(GetExampleQuery request);
    }
}
