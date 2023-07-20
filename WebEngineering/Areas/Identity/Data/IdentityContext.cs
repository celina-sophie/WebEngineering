using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebEngineering.Areas.Identity.Data;
using WebEngineering.Models;

namespace WebEngineering.Data
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public DbSet<Produkt> Produkte { get; set; }
        public DbSet<Bestellung> Bestellungen { get; set; }
        public DbSet<Lieferung> Lieferungen { get; set; }
        public DbSet<WebEngineering.Models.ViewModel> ViewModel { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IdentityContext dbContext)
        {
            // ...

            dbContext.Database.Migrate(); // Führt Migrationen durch, falls erforderlich
            DataSeeder.SeedData(dbContext); // Ruft das Datenbankseeding-Skript auf

            // ...
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Konfigurieren Sie Ihre Modelle und Beziehungen hier
            builder.Entity<ViewModel>()
                .HasOne(v => v.Lieferung)
                .WithMany()
                .HasForeignKey(v => v.Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ViewModel>()
                .HasOne(v => v.Produkt)
                .WithMany()
                .HasForeignKey(v => v.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
