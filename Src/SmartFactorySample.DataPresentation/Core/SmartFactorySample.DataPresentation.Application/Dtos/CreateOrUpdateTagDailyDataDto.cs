using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Application.Dtos
{
    public class CreateOrUpdateTagDailyDataDto
    {
        public long Id { get; set; }
        public int TagId { get; set; }

        public DateOnly Date { get; set; }
        public float? Value { get; set; }
        
    }
}
