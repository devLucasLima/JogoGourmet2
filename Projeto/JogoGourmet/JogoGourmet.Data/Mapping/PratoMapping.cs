using JogoGourmet.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace JogoGourmet.Data.Mapping
{
    public class PratoMapping : IEntityTypeConfiguration<Prato>
    {
        public void Configure(EntityTypeBuilder<Prato> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Caracteristica)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.ToTable("Pratos");
        }
    }
}
