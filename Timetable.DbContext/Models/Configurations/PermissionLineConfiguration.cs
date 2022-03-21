using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class PermissionLineConfiguration : IEntityTypeConfiguration<PermissionLine>
    {
        public void Configure(EntityTypeBuilder<PermissionLine> builder)
        {

            builder.ToTable("PermissionLines");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.DisplayName).HasMaxLength(128).IsRequired();
            builder.Property(t => t.Key).HasMaxLength(64);
            builder.HasIndex(t => t.Key).IsUnique();
            builder.Property(t => t.Url).HasMaxLength(256);
            builder.HasOne(t => t.Group).WithMany(t => t.PermissionLines).IsRequired().HasForeignKey(t => t.GroupId).OnDelete( DeleteBehavior.Restrict);
        }
    }
}
