using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class CoursePIckConfiguration : IEntityTypeConfiguration<CoursePick>
    {
        public void Configure(EntityTypeBuilder<CoursePick> builder)
        {
            builder.ToTable("CoursePIcks");
            builder.HasKey("Id");

            builder.HasOne(s => s.Course).WithMany(s => s.CoursePicks).HasForeignKey(s => s.CourseId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Student).WithMany(s => s.CoursePicks).HasForeignKey(s => s.StudentIdentityCode).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
