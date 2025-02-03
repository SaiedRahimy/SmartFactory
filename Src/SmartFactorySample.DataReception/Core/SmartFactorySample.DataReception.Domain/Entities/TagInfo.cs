using SmartFactorySample.DataReception.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Domain.Entities
{
    public class TagInfo : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
