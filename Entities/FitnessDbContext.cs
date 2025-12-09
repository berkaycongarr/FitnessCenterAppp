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
            // 1. Hizmet silinirse randevular silinmesin (Zaten vardı)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany(h => h.Randevular)
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Antrenör silinirse randevular OTOMATİK SİLİNMESİN (Hata Çözümü)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)
                .WithMany(a => a.Randevular)
                .HasForeignKey(r => r.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Kullanıcı silinirse randevular OTOMATİK SİLİNMESİN (Hata Çözümü)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.User)
                .WithMany(u => u.Randevular)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Decimal (Para) alanı ayarı (Alternatif çözüm)
            modelBuilder.Entity<Hizmet>()
                .Property(h => h.Ucret)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}