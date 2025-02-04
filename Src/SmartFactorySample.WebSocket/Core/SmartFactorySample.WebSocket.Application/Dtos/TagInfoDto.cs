using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Application.Dtos
{
    [MessagePackObject(true)]
    public class TagInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Value { get; set; }
    }

    
}
