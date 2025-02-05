using SmartFactorySample.DataPresentation.Domain.Common;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
