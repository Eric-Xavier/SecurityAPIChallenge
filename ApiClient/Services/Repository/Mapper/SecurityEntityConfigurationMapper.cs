using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiClient.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiClient.Services.Repository.Mapper
{
    public class SecurityEntityConfigurationMapper : IEntityTypeConfiguration<SecurityModel>
    {
        public void Configure(EntityTypeBuilder<SecurityModel> modelBuilder)
        {
            modelBuilder.ToTable("Securities", t=>t.ExcludeFromMigrations(false));
            modelBuilder.HasKey(x=>x.ISINCode);
            
            modelBuilder
                .Property(x=>x.ISINCode).HasMaxLength(12)
                .HasColumnType("varchar(12)")
                .HasColumnName("isin");

            modelBuilder
                .Property(x=>x.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(15,2)");
        }
    }
}