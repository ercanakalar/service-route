using System;
using System.Collections.Generic;
using System.Text;
using Backend.Core.Models.Auth;
using Backend.Core.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    public class AuthConfiguration : IEntityTypeConfiguration<AuthDto>
    {
        public void Configure(EntityTypeBuilder<AuthDto> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(m => m.Id).HasColumnName("Id");
            builder.Property(m => m.Username).HasMaxLength(100);
            builder.Property(m => m.ConfirmPassword).HasMaxLength(100);
            builder.Property(m => m.Password).HasMaxLength(100);
            builder.Property(m => m.Email).HasMaxLength(100);
            builder.Property(m => m.Roles).HasMaxLength(100);
            builder.ToTable("Auth");

            builder.HasOne(a => a.Users).WithOne(u => u.Auth).HasForeignKey<UserDto>(u => u.AuthId);
        }
    }
}
