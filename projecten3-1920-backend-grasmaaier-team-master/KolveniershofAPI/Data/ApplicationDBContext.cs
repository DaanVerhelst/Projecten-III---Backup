using Microsoft.EntityFrameworkCore;
using KolveniershofAPI.Data.Mapping;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Model_EF;
using System.Text;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KolveniershofAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext {

        public DbSet<Client> Clienten { get; set; }
        public DbSet<Begeleider> Begeleiders { get; set; }
        public DbSet<DagTemplate> Templates { get; set; }
        public DbSet<SfeerGroep> SfeerGroepen { get; set; }
        public DbSet<Atelier> Ateliers { get; set; }
        public DbSet<Atelier_Dag> Atelier_Dagen { get; set; }
        public DbSet<Dag> Dagen { get; set; }
        public DbSet<DagMenu> DagMenus { get; set; }
        public DbSet<Bus> Bussen { get; set; }
        public DbSet<Bus_Dag> Bus_Dagen { get; set; }

        /// <summary>
        /// Default constructor used to load in the configuration for EF
        /// </summary>
        /// <param name="options">Configuration only contains the default connection string</param>
        public ApplicationDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            AddConfiguration(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void AddConfiguration(ModelBuilder mb) {
            mb.Entity<Persoon>().HasKey(p => p.ID);
            mb.Entity<Persoon>().HasOne(p=>p.ProfielFoto).WithMany()
                .HasForeignKey(p => p.ProfielFotoID).IsRequired(false);

            mb.Entity<Persoon>()
                .HasDiscriminator<string>("account_type")
                .HasValue<Client>("client")
                .HasValue<Begeleider>("begeleider");

            mb.Entity<Client>().HasOne(c => c.SfeerGroep).WithMany()
                .HasForeignKey(c => c.SfeerGroepID).IsRequired(false);
            mb.Entity<Client>().HasBaseType<Persoon>();
            mb.Entity<Begeleider>().HasBaseType<Persoon>();

            mb.Entity<Notitieblok>().HasKey(nb => nb.ID);
            mb.Entity<Notitieblok>().HasMany(nb => nb.LijstNoties)
                .WithOne().IsRequired(false).OnDelete(DeleteBehavior.Cascade);

            mb.Entity<NotiteLijsten>().HasKey(nl => nl.ID);
            mb.Entity<NotiteLijsten>().HasOne(nl => nl.Persoon)
                .WithMany().IsRequired(false).OnDelete(DeleteBehavior.Cascade);
            mb.Entity<NotiteLijsten>()
                .HasDiscriminator<string>("lijst_type")
                .HasValue<IndividueleOndersteuningClienten>("ioc")
                .HasValue<BijzonderUurRegistratie>("bur");

            mb.Entity<Atelier>().HasKey(a => a.ID);
            mb.Entity<Atelier>().HasOne(a => a.Pictogram)
                .WithMany().IsRequired(false);

            mb.Entity<Bus>().HasKey(a => a.ID);
            mb.Entity<Bus>().HasOne(a => a.Pictogram)
                .WithMany().IsRequired(false);

            mb.ApplyConfiguration<Dag_Persoon>(new Dag_PersoonMapper())
              .ApplyConfiguration<Atelier_Dag>(new Atelier_DagenMapper());
              //.ApplyConfiguration<Bus_Dag>(new Bus_DagenMapper());

            mb.Entity<DagTemplate>().HasKey(dt => dt.ID);
            mb.Entity<DagTemplate>()
                .HasOne(dm => dm.Menu)
                .WithOne()
                .HasForeignKey<DagTemplate>(dt => dt.MenuID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<DagTemplate>()
                .HasMany(dt => dt.Bus_Dag)
                .WithOne(b => b.Template)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Bus_Dag>()
                .HasOne(bd => bd.Bus)
                .WithMany(b => b.BusDagen)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Bus_Dag>()
                .HasMany(bd => bd.BDP)
                .WithOne(bdp => bdp.BD)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Bus_Dag_Persoon>()
                .HasOne(bdp => bdp.Persoon)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Bus>()
                .HasOne(b => b.Pictogram)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
