using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.CourseName).IsRequired().HasMaxLength(128);

            builder.HasOne(t => t.AdminstractiveClassBackup).WithMany(s => s.Courses).HasForeignKey(t => t.AdminstractiveClassBackupId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Subject).WithMany(s => s.Courses).IsRequired().HasForeignKey(s => s.SubjectCode).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.College).WithMany(s => s.Courses).HasForeignKey(s => s.CollegeId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Department).WithMany(s => s.Courses).HasForeignKey(s => s.DepartmentId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Teacher).WithMany(s => s.Courses).HasForeignKey(t => t.TeacherIdentityCode).IsRequired().OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(s => s.AdministrativeClass).WithMany(t => t.Courses).HasForeignKey(t => t.AdministrativeClassId).OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(s => s.SchoolTerm).WithMany(s => s.Courses).HasForeignKey(s => s.SchoolTermId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        }
    }
}
