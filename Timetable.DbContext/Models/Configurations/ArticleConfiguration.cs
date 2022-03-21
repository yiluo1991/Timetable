using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Timetable.DbContext.Models.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {

   

        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Articles");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasValueGenerator<GuidValueGenerator>();
            builder.Property(t => t.Title_CN).HasMaxLength(128);
            builder.Property(t => t.Title_EN).HasMaxLength(512);
            builder.Property(t => t.Author).HasMaxLength(128);
            builder.Property(t => t.SEOKeywords_CN).HasMaxLength(128);
            builder.Property(t => t.SEOKeywords_EN).HasMaxLength(512);
            builder.Property(t => t.Description_CN).HasMaxLength(512);
            builder.Property(t => t.Description_EN).HasMaxLength(2048);
            builder.Property(t => t.Content_CN).HasMaxLength(1024 * 1024);
            builder.Property(t => t.Content_EN).HasMaxLength(1024 * 1024);
            builder.Property(t => t.Src).HasMaxLength(256);
            builder.Property(t => t.CreateTime).HasDefaultValueSql("now()");
            builder.HasOne(t => t.Channel).WithMany(t => t.Articles).HasForeignKey(t => t.ChannelId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Employee).WithMany(t => t.Articles).HasForeignKey(t => t.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
