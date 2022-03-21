using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.ToTable("Roles");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasMaxLength(32);
            builder.Property(s => s.Remark).HasMaxLength(128);
        }
    }
}
