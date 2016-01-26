using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Logging
{
    public class LogDataContext : DbContext
    {
        public LogDataContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<LogDataContext, LogDataConfiguration>());
        }

        public DbSet<ActionLog> ActionLogs { get; set; }
    }
}
