using SmartFactorySample.Simulator.Application.Common.Interfaces;
using System;

namespace SmartFactorySample.Simulator.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
