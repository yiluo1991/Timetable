using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subjects"); 
            builder.HasKey(t => t.SubjectCode);
            builder.Property(t => t.Name).HasMaxLength(128);
            builder.Property(t => t.SubjectCode).HasMaxLength(32);

            builder.HasOne(s => s.College).WithMany(s => s.Subjects).HasForeignKey(s => s.CollegeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Department).WithMany(s => s.Subjects).HasForeignKey(s => s.DepartmentId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
