using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Logging
{
    public class LogDataConfiguration
        : DbMigrationsConfiguration<LogDataContext>
    {
        public LogDataConfiguration()
        {
#if DEBUG
            this.AutomaticMigrationDataLossAllowed = true;
#endif

            this.AutomaticMigrationsEnabled = true;
        }
    }
}
