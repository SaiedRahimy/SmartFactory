using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using System;

namespace SmartFactorySample.IdentityService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
