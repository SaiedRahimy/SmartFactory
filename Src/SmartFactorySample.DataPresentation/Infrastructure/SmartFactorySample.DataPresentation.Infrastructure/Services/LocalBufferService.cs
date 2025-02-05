using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using SmartFactorySample.DataPresentation.Application.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace SmartFactorySample.DataPresentation.Infrastructure.Services
{
    public class LocalBufferService : ILocalBufferService
    {
        #region Cahe State
        private ConcurrentQueue<TagInfoDto> _influxLocalBuffer;
        private ConcurrentQueue<TagInfoDto> _sqlLocalBuffer;
        private readonly ConcurrentDictionary<(string, DateTime), List<decimal>> _hourlyData = new();
        private readonly ConcurrentDictionary<(string, DateTime), List<decimal>> _dailyData = new();

        #endregion

        #region Dependencies

        private readonly ILogger<LocalBufferService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITagDailyDataManager _tagDailyDataManager;


        #endregion

        #region Properties


        private System.Timers.Timer TimerProcessData { get; set; }

        private readonly string InfluxDbUrl = string.Empty;
        private readonly string InfluxDbToken = string.Empty;
        private readonly string InfluxDbOrg = string.Empty;
        private readonly string BucketDaily = string.Empty;
        private readonly string BucketMonthly = string.Empty;
        private readonly string BucketYearly = string.Empty;


        #endregion

        #region Constructor
        public LocalBufferService(ILogger<LocalBufferService> logger, IConfiguration configuration, ITagDailyDataManager tagDailyDataManager)
        {
            _logger = logger;
            _tagDailyDataManager = tagDailyDataManager;


            _influxLocalBuffer = new ConcurrentQueue<TagInfoDto>();
            _sqlLocalBuffer = new ConcurrentQueue<TagInfoDto>();

            InfluxDbUrl = configuration["ConnectionStrings:InfluxDB"];
            InfluxDbToken = configuration["InfluxDbToken"];
            InfluxDbOrg = configuration["InfluxDbOrg"];
            BucketDaily = configuration["BucketDaily"];
            BucketMonthly = configuration["BucketMonthly"];
            BucketYearly = configuration["BucketYearly"];



            StartTimerProcessData();

            Task.Run(ProcessSensorData);
            Task.Run(AggregateHourlyData);
            Task.Run(AggregateDailyData);
        }
        #endregion

        #region Public Methods

        public void Push(TagInfoDto tag)
        {
            _influxLocalBuffer.Enqueue(tag);
            _sqlLocalBuffer.Enqueue(tag);
        }


        #endregion

        #region private Methods


        private List<TagInfoDto> TakeFromLocalCache(int listCount)
        {

            var tags = new List<TagInfoDto>();

            if (_sqlLocalBuffer.Count > 0)
            {
                var count = Math.Min(listCount, _sqlLocalBuffer.Count);
                while (count >= 0 && _sqlLocalBuffer.Count > 0)
                {
                    if (_sqlLocalBuffer.TryDequeue(out TagInfoDto dto))
                    {
                        count--;
                        tags.Add(dto);
                    }
                }

            }
            return tags;
        }

        private async Task RegisterDataInSql(List<TagInfoDto> tags)
        {
            if (tags?.Count > 0)
            {
                try
                {
                    await _tagDailyDataManager.CreateOrUpdate(tags);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception in ProcessLocalQueueData", ex);
                }
            }
        }

       

        void StartTimerProcessData()
        {
            TimerProcessData = new System.Timers.Timer(500);
            TimerProcessData.Elapsed += async (a, b) =>
            {
                if (_sqlLocalBuffer.Count > 0)
                {
                    var dataList = TakeFromLocalCache(1000);

                    await RegisterDataInSql(dataList);

                }
            };
            TimerProcessData.Enabled = true;
        }

        private async Task ProcessSensorData()
        {
            while (true)
            {
                using var client = InfluxDBClientFactory.Create(InfluxDbUrl, InfluxDbToken);
                var writeApi = client.GetWriteApiAsync();
                try
                {

                    if (_influxLocalBuffer.TryDequeue(out var data))
                    {
                        var point = PointData
                            .Measurement("sensor_data")
                            .Tag("sensor", data.Name)
                            .Field("value", (double)data.Value)
                            .Timestamp(data.Timestamp, WritePrecision.Ns);

                        await writeApi.WritePointAsync(point, BucketDaily, InfluxDbOrg);

                        // ذخیره برای پردازش ساعتی و روزانه
                        var hourKey = (data.Name, data.Timestamp.Date.AddHours(data.Timestamp.Hour));
                        _hourlyData.AddOrUpdate(hourKey, new List<decimal> { data.Value }, (_, list) => { list.Add(data.Value); return list; });

                        var dayKey = (data.Name, data.Timestamp.Date);
                        _dailyData.AddOrUpdate(dayKey, new List<decimal> { data.Value }, (_, list) => { list.Add(data.Value); return list; });

                        _logger.LogInformation($"Store Data : {data.Name} - {data.Value}");
                    }

                    await Task.Delay(50);

                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in AggregateHourlyData : ", ex);

                }
            }
        }

        private async Task AggregateHourlyData()
        {
            while (true)
            {
                using var client = InfluxDBClientFactory.Create(InfluxDbUrl, InfluxDbToken);
                var writeApi = client.GetWriteApiAsync();
                try
                {
                    var now = DateTime.UtcNow;
                    foreach (var key in _hourlyData.Keys)
                    {
                        if (key.Item2 <= now.AddHours(-1))
                        {
                            if (_hourlyData.TryRemove(key, out var values))
                            {
                                var avg = (double)values.Average();
                                var min = (double)values.Min();
                                var max = (double)values.Max();

                                var point = PointData
                                    .Measurement("sensor_hourly")
                                    .Tag("sensor", key.Item1)
                                    .Field("average", avg)
                                    .Field("minimum", min)
                                    .Field("maximum", max)
                                    .Timestamp(key.Item2, WritePrecision.Ns);

                                await writeApi.WritePointAsync(point, BucketMonthly, InfluxDbOrg);
                                _logger.LogInformation($"Store Data Hourly: {key.Item1} - Avg: {avg}, Min: {min}, Max: {max}");
                            }
                        }
                    }
                    await Task.Delay(1000 * 60 * 5);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in AggregateHourlyData : ", ex);

                }
            }
        }

        private async Task AggregateDailyData()
        {
            while (true)
            {
                using var client = InfluxDBClientFactory.Create(InfluxDbUrl, InfluxDbToken);
                var writeApi = client.GetWriteApiAsync();
                try
                {
                    var now = DateTime.UtcNow;
                    foreach (var key in _dailyData.Keys)
                    {
                        if (key.Item2 <= now.AddDays(-1))
                        {
                            if (_dailyData.TryRemove(key, out var values))
                            {
                                var avg = (double)values.Average();
                                var min = (double)values.Min();
                                var max = (double)values.Max();

                                var point = PointData
                                    .Measurement("sensor_daily")
                                    .Tag("sensor", key.Item1)
                                    .Field("average", avg)
                                    .Field("minimum", min)
                                    .Field("maximum", max)
                                    .Timestamp(key.Item2, WritePrecision.Ns);

                                await writeApi.WritePointAsync(point, BucketYearly, InfluxDbOrg);
                                _logger.LogInformation($"Store Data Daily: {key.Item1} - Avg: {avg}, Min: {min}, Max: {max}");
                            }
                        }
                    }
                    await Task.Delay(1000 * 60 * 60); // هر 1 ساعت چک کند
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in AggregateDailyData : ", ex);

                }
            }

            #endregion
        }
    }
}
