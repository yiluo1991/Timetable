using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> builder)
        {

            builder.ToTable("EmployeeRoles");
            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.Role).WithMany(s => s.EmployeeRoles).IsRequired().HasForeignKey(s => s.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Employee).WithMany(s => s.EmployeeRoles).IsRequired().HasForeignKey(s => s.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
