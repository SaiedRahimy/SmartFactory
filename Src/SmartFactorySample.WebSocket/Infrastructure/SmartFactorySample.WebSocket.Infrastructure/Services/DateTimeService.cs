using SmartFactorySample.WebSocket.Application.Common.Interfaces;
using System;

namespace SmartFactorySample.WebSocket.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
