using KolveniershofAPI.Model.Model_EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Mapping
{
    public class Bus_DagenMapper : IEntityTypeConfiguration<Bus_Dag>
    {
        public void Configure(EntityTypeBuilder<Bus_Dag> builder)
        {
            builder.ToTable("Bus_Dagen");

            builder.HasKey(k => k.ID);

            builder
                .HasOne(ad => ad.Template)
                .WithMany(d => d.Bus_Dag)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(ad => ad.Bus)
                .WithMany(a => a.BusDagen)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(ad => ad.BDP)
                .WithOne(adp => adp.BD)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
