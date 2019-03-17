using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitbit.Models;
using mmangold.com.Models;
using Microsoft.EntityFrameworkCore;
using WaniKaniApi.Models;

namespace mmangold.com
{
    public class SiteDbContext : DbContext
    {
        public SiteDbContext(DbContextOptions<SiteDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<WaniKaniRadicalItemLocal>()
            //    .HasIndex(r => r.ImageUri)
            //    .IsUnique();
            //builder.Entity<WaniKaniKanjiItemLocal>()
            //    .HasIndex(r => r.Character)
            //    .IsUnique();
            //builder.Entity<WaniKaniVocabularyItemLocal>()
            //    .HasIndex(r => r.Character)
            //    .IsUnique();
            builder.Entity<GuruOrGreaterRadical>()
                .HasIndex(r => r.ImageUri)
                .IsUnique();
            builder.Entity<GuruOrGreaterKanji>()
                .HasIndex(r => r.Character)
                .IsUnique();
            builder.Entity<GuruOrGreaterVocab>()
                .HasIndex(r => r.Character)
                .IsUnique();
            //builder.Entity<SimpleWeightLog>()
            //    .HasIndex(r => r.Date)
            //    .IsUnique();
        }

        //public DbSet<WeightLogLocal> WeightLogs { get; set; }
        //public DbSet<WaniKaniRadicalItemLocal> RadicalItems { get; set; }
        //public DbSet<WaniKaniKanjiItemLocal> KanjiItems { get; set; }
        //public DbSet<WaniKaniVocabularyItemLocal> VocabularyItems { get; set; }
        public DbSet<GuruOrGreaterRadical> GuruOrGreaterRadicals { get; set; }
        public DbSet<GuruOrGreaterKanji> GuruOrGreaterKanjis { get; set; }
        public DbSet<GuruOrGreaterVocab> GuruOrGreaterVocabs { get; set; }
        public DbSet<SimpleWeightLog> SimpleWeightLogs { get; set; }
        public DbSet<WeightSync> WeightSyncs { get; set; }
        public DbSet<WaniKaniSync> WaniKaniSyncs { get; set; }
        

    }
}
