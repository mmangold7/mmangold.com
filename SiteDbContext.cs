using Microsoft.EntityFrameworkCore;
using mmangold.com.Models;
using WaniKaniApi.Models;

namespace mmangold.com
{
    public class SiteDbContext : DbContext
    {
        public SiteDbContext(DbContextOptions<SiteDbContext> options) : base(options)
        {
        }

        public DbSet<GuruOrGreaterRadical> GuruOrGreaterRadicals { get; set; }
        public DbSet<GuruOrGreaterKanji> GuruOrGreaterKanjis { get; set; }
        public DbSet<GuruOrGreaterVocab> GuruOrGreaterVocabs { get; set; }
        public DbSet<SimpleWeightLog> SimpleWeightLogs { get; set; }
        public DbSet<WeightSync> WeightSyncs { get; set; }
        public DbSet<WaniKaniSync> WaniKaniSyncs { get; set; }
        public DbSet<LevelProgress> LevelProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GuruOrGreaterRadical>()
                .HasIndex(r => r.ImageUri)
                .IsUnique();
            builder.Entity<GuruOrGreaterKanji>()
                .HasIndex(r => r.Character)
                .IsUnique();
            builder.Entity<GuruOrGreaterVocab>()
                .HasIndex(r => r.Character)
                .IsUnique();
        }
    }
}