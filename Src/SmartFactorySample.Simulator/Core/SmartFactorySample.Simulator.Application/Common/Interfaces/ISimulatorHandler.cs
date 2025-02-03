using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Common.Interfaces
{
    public interface ISimulatorHandler
    {
        public Task StartAsync();
        public Task StopAsync();
    }
}
