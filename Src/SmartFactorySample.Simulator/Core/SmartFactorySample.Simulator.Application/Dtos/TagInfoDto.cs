using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.Simulator.Application.Dtos
{
    [MessagePackObject(true)]
    public class TagInfoDto
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Value { get; set; }
    }
}
