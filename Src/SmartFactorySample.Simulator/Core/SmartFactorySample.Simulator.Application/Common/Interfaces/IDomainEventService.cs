using SmartFactorySample.Simulator.Domain.Common;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
