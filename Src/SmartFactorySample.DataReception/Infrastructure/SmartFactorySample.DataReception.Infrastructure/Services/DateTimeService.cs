using SmartFactorySample.DataReception.Application.Common.Interfaces;
using System;

namespace SmartFactorySample.DataReception.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
