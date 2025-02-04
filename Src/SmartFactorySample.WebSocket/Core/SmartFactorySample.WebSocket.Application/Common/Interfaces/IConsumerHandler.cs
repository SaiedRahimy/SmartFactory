using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Application.Common.Interfaces
{
    public interface IConsumerHandler
    {
        public Task StartAsync();
        public Task StopAsync();
    }
}
