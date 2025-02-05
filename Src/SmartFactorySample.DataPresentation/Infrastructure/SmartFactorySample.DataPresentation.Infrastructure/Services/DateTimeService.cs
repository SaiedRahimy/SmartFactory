using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using System;

namespace SmartFactorySample.DataPresentation.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
