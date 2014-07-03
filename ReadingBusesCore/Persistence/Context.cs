using ReadingBusesCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingBusesCore.Routes.Entities;
using ReadingBusesCore.Persistence.Entities;

namespace ReadingBusesCore.Persistence
{
    public class Context : DbContext
    {
      //Can be set if it needs to be modified
       public static string ConnectionStringName = "DefaultConnection";
 
       public Context()
          : base(ConnectionStringName)
       { }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<ServicePattern> ServicePatterns { get; set; }

        public DbSet<LastUpdate> LastUpdates { get; set; }

        //This is required for auto migrations
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
