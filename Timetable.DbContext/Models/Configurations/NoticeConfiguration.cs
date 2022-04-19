using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class NoticeConfiguration : IEntityTypeConfiguration<Notice>
    {
        public void Configure(EntityTypeBuilder<Notice> builder)
        {
            builder.ToTable("Notices");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Content).IsRequired().HasMaxLength(4096);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(256);

        }
    }
}
