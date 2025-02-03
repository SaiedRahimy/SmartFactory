using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
