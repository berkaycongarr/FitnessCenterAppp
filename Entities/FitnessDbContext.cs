using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Entities
{
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext(DbContextOptions<FitnessDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<SporSalonu> SporSalonlari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Hizmet silindiğinde geçmiş randevular bozulmasın diye (Restrict)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany(h => h.Randevular)
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}