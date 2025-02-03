using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartFactorySample.Simulator.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Infrastructure.Services
{
    public class ProcessorHostedService : IHostedService
    {
        #region Dependencies
        private readonly ISimulatorHandler _processorHandler;
        private readonly ILogger<ProcessorHostedService> _logger;
        private Task _processingTask;
        #endregion

        #region Constructor
        public ProcessorHostedService(ISimulatorHandler processorHandler, ILogger<ProcessorHostedService> logger)
        {
            _processorHandler = processorHandler;
            _logger = logger;
        }
        #endregion

        #region Hosted Methods

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ProcessorHostedService is starting.");
            await _processorHandler.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ProcessorHostedService is stopping.");
            await _processorHandler.StopAsync();
        }
        #endregion
    }
}
