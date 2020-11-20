using Microsoft.EntityFrameworkCore;
using Piovra.EfCore.Extensions;

namespace Piovra.Demo.EfCoreTests {
    public class StaffDc : DbContext {
        public StaffDc(DbContextOptions<StaffDc> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);
            MapPerson(modelBuilder);
        }

        static void MapPerson(ModelBuilder modelBuilder) {
            var cfg = modelBuilder.Entity<Person>();
            cfg.ToTable("Person", "public");

            cfg.Property(x => x.Id).HasColumnName("id");
            cfg.Property(x => x.Info)
                .HasColumnName("info")
                .HasColumnType("xml")
                .HasConversion(new XmlPropertyConverter("Info"));

            cfg.HasKey(x => x.Id);
        }
    }
}
