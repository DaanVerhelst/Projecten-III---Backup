using Microsoft.EntityFrameworkCore;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofAPI.Data.Mapping
{
    public class Dag_PersoonMapper : IEntityTypeConfiguration<Dag_Persoon>{
        public void Configure(EntityTypeBuilder<Dag_Persoon> builder){
            builder.HasKey(dp => dp.ID);

            builder
                .HasOne(dp => dp.Persoon)
                .WithMany( p=> p.Dag_Personen)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder
                .HasOne(dp => dp.Dag)
                .WithMany(d=>d.Dag_Personen)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
