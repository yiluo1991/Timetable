using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class CollegeConfiguration : IEntityTypeConfiguration<College>
    {
        public void Configure(EntityTypeBuilder<College> builder)
        {
            builder.ToTable("Colleges");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(128).IsRequired();
            builder.Property(t => t.Remark).HasMaxLength(512).IsRequired();
            builder.Property(t => t.ContactName).HasMaxLength(64).IsRequired(); 
            builder.Property(t => t.ContactPhone).HasMaxLength(32).IsRequired(); 
        }
    }
}
