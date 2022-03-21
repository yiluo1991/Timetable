using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public  class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            
            builder.HasKey(t => t.Id);
            builder.Property(t => t.LoginName).IsRequired().HasMaxLength(128);
            builder.HasIndex(t => t.LoginName).IsUnique(true);
            builder.Property(t => t.Password).IsRequired().HasMaxLength(128);
            builder.Property(t => t.Headshot).HasMaxLength(256);
            builder.Property(t => t.Email).HasMaxLength(128);
            builder.Property(t => t.RealName).HasMaxLength(64);
            builder.Property(t => t.Address).HasMaxLength(256);
            builder.Property(t => t.Mobile).HasMaxLength(16);
            builder.Property(t => t.SinglePointHsah).HasMaxLength(16);
            builder.HasOne(t => t.Creator).WithMany().IsRequired(false).HasForeignKey(s => s.CreatorId).OnDelete(DeleteBehavior.Restrict);;
        }


    }
}
