using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class AdministrativeClassBackupConfiguration : IEntityTypeConfiguration<AdministrativeClassBackup>
    {
        public void Configure(EntityTypeBuilder<AdministrativeClassBackup> builder)
        {
            builder.ToTable("AdministrativeClassBackups");
            builder.HasKey(t => t.Id);

            builder.HasOne(s => s.AdministrativeClass).WithMany(s => s.AdministrativeClassBackups).IsRequired().HasForeignKey(t => t.AdminstractiveClassId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
