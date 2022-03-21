using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public  class EmployeeLoginLogConfiguration : IEntityTypeConfiguration<EmployeeLoginLog>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoginLog> builder)
        {
            builder.ToTable("EmployeeLoginLogs");
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Employee).WithMany(t => t.EmployeeLoginLogs).HasForeignKey(s => s.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
