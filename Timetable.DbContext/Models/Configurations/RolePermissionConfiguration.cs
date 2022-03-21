using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {

            builder.ToTable("RolePermissions");
            builder.HasKey(t => t.Id);
            builder.HasOne(s => s.PermissionLine).WithMany(s => s.RolePermissions).IsRequired().HasForeignKey(s => s.PermissionLineId).OnDelete( DeleteBehavior.Restrict);
            builder.HasOne(s => s.Role).WithMany(s => s.RolePermissions).IsRequired().HasForeignKey(s => s.RoleId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
