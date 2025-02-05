using SmartFactorySample.DataPresentation.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Domain.Entities
{
    public class TagDailyData : AuditableEntity
    {
        public long Id { get; set; }
        public int TagId { get; set; }
        
        public DateOnly Date { get; set; }
        public float? Key1 { get; set; }
        public float? Key2 { get; set; }
        public float? Key3 { get; set; }
        public float? Key4 { get; set; }
        public float? Key5 { get; set; }
        public float? Key6 { get; set; }
        public float? Key7 { get; set; }
        public float? Key8 { get; set; }
        public float? Key9 { get; set; }
        public float? Key10 { get; set; }
    }
}
