using System;
using System.Collections.Generic;
using System.Text;
using Backend.Core.Models.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<CompanyDto>
    {
        public void Configure(EntityTypeBuilder<CompanyDto> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Name).HasMaxLength(100);
            builder.Property(m => m.Address).HasMaxLength(100);
            builder.Property(m => m.City).HasMaxLength(100);
            builder.Property(m => m.Country).HasMaxLength(100);
            builder.Property(m => m.Email).HasMaxLength(100);
            builder.Property(m => m.Phone).HasMaxLength(100);
            builder.Property(m => m.Website).HasMaxLength(100);
            builder.ToTable("Companies");
        }
    }
}
