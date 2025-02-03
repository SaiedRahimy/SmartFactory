using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Common.Interfaces
{
    public interface IMessageQueueService
    {
        Task PublishAsync<T>(T message);
        Task<T> ConsumeAsync<T>();
    }
}
