using SmartFactorySample.IdentityService.Domain.Common;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
