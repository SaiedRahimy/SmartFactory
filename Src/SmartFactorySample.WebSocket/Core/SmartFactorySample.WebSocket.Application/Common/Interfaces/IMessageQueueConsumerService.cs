using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Application.Common.Interfaces
{
    public interface IMessageQueueConsumerService
    {
        void ConsumeData();
        void Stop();
    }
}
