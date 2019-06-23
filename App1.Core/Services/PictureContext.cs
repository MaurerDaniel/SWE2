using App1.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace App1.Core.Services
{
    class PictureContext : DbContext
    {
        DbSet<PictureModel> Pictures { get; set; }
        DbSet<FotographerModel> Fotographers { get; set; }


        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=People.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Id required.
            modelBuilder.Entity<PictureModel>()
                .Property(p => p.Id)
                .IsRequired();

            // Make Name required.
            modelBuilder.Entity<PictureModel>()
                .Property(p => p.Name)
                .IsRequired();

            // Make Id required.
            modelBuilder.Entity<FotographerModel>()
                .Property(p => p.ID)
                .IsRequired();

            // Make Name required.
            modelBuilder.Entity<FotographerModel>()
                .Property(p => p.Name)
                .IsRequired();
        }
    }
}
