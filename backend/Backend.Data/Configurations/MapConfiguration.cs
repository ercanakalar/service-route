using System;
using System.Collections.Generic;
using System.Text;
using Backend.Core.Models.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    public class MapConfiguration : IEntityTypeConfiguration<WaypointsDto>
    {
        public void Configure(EntityTypeBuilder<WaypointsDto> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Address).HasMaxLength(100);
            builder.Property(m => m.Latitude);
            builder.Property(m => m.Longitude);
            builder.Property(m => m.Order);
            builder.Property(m => m.CreatedAt);
            builder.Property(m => m.UpdatedAt);
            builder.ToTable("Waypoints");

            builder.HasOne(w => w.Company).WithMany().HasForeignKey(w => w.CompanyId);
        }
    }
}
