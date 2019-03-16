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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Ignore<WaniKaniItemUserInfo.UserSynonyms>();
        //}

        public DbSet<WeightLogLocal> WeightLogs { get; set; }
        public DbSet<WaniKaniRadicalItemLocal> RadicalItems { get; set; }
        public DbSet<WaniKaniKanjiItemLocal> KanjiItems { get; set; }
        public DbSet<WaniKaniVocabularyItemLocal> VocabularyItems { get; set; }

    }

    //public class Blog
    //{
    //    public int BlogId { get; set; }
    //    public string Url { get; set; }

    //    public ICollection<Post> Posts { get; set; }
    //}

    //public class Post
    //{
    //    public int PostId { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }

    //    public int BlogId { get; set; }
    //    public Blog Blog { get; set; }
    //}
}
