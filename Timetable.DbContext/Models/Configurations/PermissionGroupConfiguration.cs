using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {

            builder.ToTable("PermissionGroups");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.DisplayName).HasMaxLength(128).IsRequired();
            builder.Property(t => t.Url).HasMaxLength(256);
            builder.Property(t => t.Key).HasMaxLength(64);
            builder.HasIndex(t => t.Key).IsUnique();
            builder.Property(t => t.Headshot).HasMaxLength(256);
            builder.HasOne(t => t.Parent).WithMany(t => t.Children).IsRequired(false).HasForeignKey(t => t.ParentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
