using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PickupGameBot.Entities;

namespace PickupGameBot.Databases
{
    public class ChannelDatabase : DbContext
    {
        public DbSet<ChannelConfig> ChannelConfigs { get; set; }

        public ChannelDatabase()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string baseDir = Path.Combine(AppContext.BaseDirectory, "data");
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            string datadir = Path.Combine(baseDir, "channels.sqlite.db");
            optionsBuilder.UseSqlite($"Filename={datadir}");
        }

        public ChannelConfig GetChannelConfig(ulong channelId) 
            => ChannelConfigs.SingleOrDefault(c => c.ChannelId == channelId);

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            builder.Entity<PickupChannel>()
//                .Property(x => x.)
//        }
    }
}