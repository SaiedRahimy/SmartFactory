using SmartFactorySample.Simulator.Application.Dtos;
using SmartFactorySample.Simulator.Application.Entities.Example.Commands.CreateExample;
using SmartFactorySample.Simulator.Application.Entities.Example.Commands.DeleteExample;
using SmartFactorySample.Simulator.Application.Entities.Example.Commands.UpdateExample;
using SmartFactorySample.Simulator.Application.Entities.Example.Queries.GetExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Common.Interfaces
{
    public interface IExampleManager
    {
        public bool CreateExample(CreateExampleCommand request);
        public bool UpdateExample(UpdateExampleCommand request);
        public bool DeleteExample(DeleteExampleCommand request);
        public ExampleDto GetExample(GetExampleQuery request);
    }
}
