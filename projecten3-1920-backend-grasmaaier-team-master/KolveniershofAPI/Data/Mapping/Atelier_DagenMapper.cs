using Microsoft.EntityFrameworkCore;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofAPI.Data.Mapping{
    public class Atelier_DagenMapper : IEntityTypeConfiguration<Atelier_Dag>{
        public void Configure(EntityTypeBuilder<Atelier_Dag> builder){
            builder.ToTable("Atelier_Dagen");

            builder.HasKey(k => k.ID);

            builder
                .HasOne(ad => ad.Template)
                .WithMany(d => d.Atelier_Dag)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(ad => ad.Atelier)
                .WithMany(a => a.AtelierDagen)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(ad => ad.ADP)
                .WithOne(adp => adp.AD)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
