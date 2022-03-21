using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models.Configurations
{
    public class SchoolTermConfiguration : IEntityTypeConfiguration<SchoolTerm>
    {
        public void Configure(EntityTypeBuilder<SchoolTerm> builder)
        {
            builder.ToTable("SchoolTerm");
            builder.HasKey("Id");
           
        }
    }
}
