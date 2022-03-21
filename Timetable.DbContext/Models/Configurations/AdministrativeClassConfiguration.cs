using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class AdministrativeClassConfiguration : IEntityTypeConfiguration<AdministrativeClass>
    {
        public void Configure(EntityTypeBuilder<AdministrativeClass> builder)
        {
            builder.ToTable("AdministrativeClasses");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.FullName).HasMaxLength(128).IsRequired();

            builder.HasOne(s => s.College).WithMany(s => s.AdministrativeClasses).HasForeignKey(t => t.CollegeId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Department).WithMany(s => s.AdministrativeClasses).HasForeignKey(s => s.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
