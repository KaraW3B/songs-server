using KaraWeb.Core.Helpers;
using KaraWeb.Core.Models.Collections;
using KaraWeb.Core.Models.Songs;
using Microsoft.EntityFrameworkCore;

namespace KaraWeb.Core
{
    public sealed class KaraWebDbContext : DbContext
    {
        public DbSet<Collection> Collections { get; set; }

        public DbSet<Song> Songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Constants.DbFilePath}");
        }
    }
}
