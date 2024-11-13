using System;
using System.Collections.Generic;
using System.Text;
using Backend.Core.Models.Auth;
using Backend.Core.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDto>
    {
        public void Configure(EntityTypeBuilder<UserDto> builder)
        {
            builder.HasKey(u => u.Id);
            builder.ToTable("Users");

            builder.HasOne(u => u.Auth).WithOne(a => a.Users).HasForeignKey<UserDto>(u => u.AuthId);
        }
    }
}
