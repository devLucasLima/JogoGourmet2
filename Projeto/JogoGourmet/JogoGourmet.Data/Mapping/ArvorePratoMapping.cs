using JogoGourmet.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JogoGourmet.Data.Mapping
{
    public class ArvorePratoMapping : IEntityTypeConfiguration<ArvorePrato>
    {
        public void Configure(EntityTypeBuilder<ArvorePrato> builder)
        {
            builder.HasKey(n => n.Id);

            // 1 : 1 => Node ArvorePrato : Prato
            builder.HasOne(n => n.Prato)
           .WithOne(p => p.ArvorePrato)
           .HasForeignKey<ArvorePrato>(n => n.PratoId);

            builder.HasOne(n => n.NodeComCaracteristica)
            .WithOne(n => n.NodePaiComCaracteristica)
            .HasForeignKey<ArvorePrato>(n => n.NodeComCaracteristicaId);

            builder.HasOne(n => n.NodeSemCaracteristica)
           .WithOne(n => n.NodePaiSemCaracteristica)
           .HasForeignKey<ArvorePrato>(n => n.NodeSemCaracteristicaId);

            builder.ToTable("ArvorePratos");
        }
    }
}
