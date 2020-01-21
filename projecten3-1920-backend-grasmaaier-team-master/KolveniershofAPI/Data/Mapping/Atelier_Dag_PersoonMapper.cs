using KolveniershofAPI.Model.Model_EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofAPI.Data.Mapping{
    public class Atelier_Dag_PersoonMapper : IEntityTypeConfiguration<Atelier_Dag_Persoon>{
        public void Configure(EntityTypeBuilder<Atelier_Dag_Persoon> builder){
            builder.HasKey(adp => adp.ID);

            builder
                .HasOne(adp => adp.Persoon)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(adp => adp.AD)
                .WithMany(ad => ad.ADP)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
