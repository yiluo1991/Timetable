using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");
            builder.HasKey(t => t.IdentityCode); 
            builder.Property(t => t.IdentityCode).HasMaxLength(32);
            builder.Property(t => t.SchoolOpenId).HasMaxLength(64);
            builder.HasIndex(t => t.SchoolOpenId).IsUnique();
            builder.Property(t => t.Password).HasMaxLength(128);
            builder.Property(t => t.SinglePointHash).HasMaxLength(16);
            builder.Property(t => t.RealName).HasMaxLength(64);
            builder.Property(t => t.AvatarUrl).HasMaxLength(256);
            builder.Property(t => t.Mobile).HasMaxLength(16);


        }
    }
}
