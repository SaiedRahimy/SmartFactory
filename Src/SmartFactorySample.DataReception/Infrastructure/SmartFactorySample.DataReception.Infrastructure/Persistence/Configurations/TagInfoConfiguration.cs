using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartFactorySample.DataReception.Domain.Entities;

namespace SmartFactorySample.DataReception.Infrastructure.Persistence.Configurations
{
    public class TagInfoConfiguration : IEntityTypeConfiguration<TagInfo>
    {
        public void Configure(EntityTypeBuilder<TagInfo> builder)
        {
            builder.ToTable("TagInfos", "Data");

            builder.HasKey(tagInfo => tagInfo.Id);
            builder.Property(tagInfo => tagInfo.Id).ValueGeneratedOnAdd();

            builder.Property(tagInfo => tagInfo.Name).IsRequired();
            builder.HasIndex(tagInfo => tagInfo.Name).IsUnique();

            builder.Property(tagInfo => tagInfo.Created).HasDefaultValueSql("getdate()").ValueGeneratedOnAdd();
            builder.Property(tagInfo => tagInfo.LastModified).HasDefaultValueSql("getdate()").ValueGeneratedOnUpdate();
        }
    }
}
