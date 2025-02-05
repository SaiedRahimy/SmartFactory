using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using SmartFactorySample.IdentityService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Infrastructure.Services
{
    public class SimulatorHandler : ISimulatorHandler
    {
        #region Dependencies
        private readonly ILogger<SimulatorHandler> _logger;
        private readonly IMessageQueueService _messageQueueService;

        #endregion

        #region Properties

        private bool _isRunning = true;
        private int _tagsCount = 0;
        private int _simulationInterval = 0;
        private List<string> _tagsName = new List<string>();
        private System.Timers.Timer _genratorTimer;

        #endregion

        #region Constructor
        public SimulatorHandler(IMessageQueueService messageQueueService, ILogger<SimulatorHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _messageQueueService = messageQueueService;
            _tagsCount = int.Parse(configuration["TagsCount"]);
            _simulationInterval = int.Parse(configuration["SimulationInterval"]);

        }
        #endregion

        #region Public Methods
        public async Task StartAsync()
        {
            _logger.LogInformation("SimulatorHandler started.");


            CreateTagsName();

            _genratorTimer = new System.Timers.Timer(TimeSpan.FromSeconds(_simulationInterval).TotalMilliseconds);
            _genratorTimer.Enabled = true;
            _genratorTimer.Elapsed += async (x, y) =>
            {
                await GenerateTagsValue();
            };

        }

        public async Task StopAsync()
        {
            _logger.LogInformation("SimulatorHandler stopping...");
            _isRunning = false;
            _genratorTimer.Enabled = false;
        }

        #endregion


        #region Private Methods

        private void CreateTagsName()
        {
            for (int i = 0; i < _tagsCount; i++)
            {
                _tagsName.Add($"SimulatorTag.Area.Sample{i}");
            }
        }


        private async Task GenerateTagsValue()
        {
            var tasks = new List<Task>();
            foreach (var name in _tagsName)
            {

                tasks.Add(Task.Run(() => _messageQueueService.PublishAsync(new TagInfoDto
                {
                    Name = name,
                    Timestamp = DateTime.UtcNow,
                    Value = (decimal)(Random.Shared.NextDouble() * 100)

                })));

                await Task.WhenAll(tasks);
            }
        }




        #endregion
    }
}
