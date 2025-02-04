using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Application.Common.Interfaces
{
    public interface IMessageQueueConsumerService
    {
        void ConsumeData();
        void Stop();
    }
}
