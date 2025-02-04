using SmartFactorySample.WebSocket.Domain.Common;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
