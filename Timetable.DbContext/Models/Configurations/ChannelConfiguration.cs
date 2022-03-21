using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Timetable.DbContext.Models.Configurations
{
    public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {



        public void Configure(EntityTypeBuilder<Channel> builder)
        {

            builder.ToTable("Channels");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasValueGenerator<GuidValueGenerator>();
            builder.Property(t => t.Name_CN).HasMaxLength(32);
            builder.Property(t => t.Name_EN).HasMaxLength(128);
            builder.Property(t => t.SEOKeywords_CN).HasMaxLength(128);
            builder.Property(t => t.SEOKeywords_EN).HasMaxLength(512);
            builder.Property(t => t.SpecialUrl).HasMaxLength(256);
            builder.Property(t => t.Description_CN).HasMaxLength(512);
            builder.Property(t => t.Description_EN).HasMaxLength(2048);
            builder.Property(t => t.Key).HasMaxLength(64);
            builder.HasIndex(t => t.Key).IsUnique(true);
            builder.HasOne(t => t.Parent).WithMany(t => t.Children).HasForeignKey(s => s.ParentId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
