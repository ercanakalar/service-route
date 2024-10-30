using System;
using System.Collections.Generic;
using System.Text;
using Backend.Core.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(m => m.Id).HasColumnName("ID");
            builder.Property(m => m.Username).HasMaxLength(100);
            builder.Property(m => m.ConfirmPassword).HasMaxLength(100);
            builder.Property(m => m.Password).HasMaxLength(100);
            builder.Property(m => m.Email).HasMaxLength(100);
            builder.Property(m => m.Roles).HasMaxLength(100);
            builder.ToTable("Users");
        }
    }
}
