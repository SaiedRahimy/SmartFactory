using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFactorySample.DataPresentation.Domain.Entities;

namespace SmartFactorySample.DataPresentation.Infrastructure.Persistence.Configurations
{
    public class TagDailyDataConfiguration : IEntityTypeConfiguration<Domain.Entities.TagDailyData>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.TagDailyData> builder)
        {
            builder.ToTable("TagDailyDatas", "Data");

            builder.HasKey(tagDailyData => tagDailyData.Id);
            builder.Property(tagDailyData => tagDailyData.Id).ValueGeneratedOnAdd();

            builder.Property(tagDailyData => tagDailyData.TagId).IsRequired();
            builder.Property(tagDailyData => tagDailyData.Date).IsRequired();
            builder.HasIndex(tagDailyData => new { tagDailyData.TagId, tagDailyData.Date }).IsUnique();

            builder.Property(tagDailyData => tagDailyData.Created).HasDefaultValueSql("getdate()").ValueGeneratedOnAdd();
            builder.Property(tagDailyData => tagDailyData.LastModified).HasDefaultValueSql("getdate()").ValueGeneratedOnUpdate();
        }
    }
}
