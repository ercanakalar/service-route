using System;
using System.Collections.Generic;
using System.Text;
using Backend.Core.Models.Auth;
using Backend.Core.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.ToTable("User");

            builder.HasOne(u => u.Auth).WithOne(a => a.User).HasForeignKey<User>(u => u.AuthId);
        }
    }
}
