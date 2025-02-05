using SmartFactorySample.IdentityService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Domain.Entities
{
    public class Example : AuditableEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
