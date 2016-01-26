using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Logging
{
    public interface ILogRepository
    {
        ActionLog GetActionLog(int id);
        IQueryable<ActionLog> GetLogsFromDate(DateTime dateTime);
        void AddActionLog(ActionLog log);

        bool Save();
    }
}
