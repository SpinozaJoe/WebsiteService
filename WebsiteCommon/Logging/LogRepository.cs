using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Logging
{
    public class LogRepository : ILogRepository
    {
        private readonly LogDataContext m_context;

        public LogRepository(LogDataContext context)
        {
            m_context = context;
        }

        public ActionLog GetActionLog(int id)
        {
            return m_context.ActionLogs.Where(l => l.Id == id).FirstOrDefault();
        }

        public IQueryable<ActionLog> GetLogsFromDate(DateTime dateTime)
        {
            return m_context.ActionLogs.Where(l => l.TimeStamp >= dateTime);
        }

        public void AddActionLog(ActionLog log)
        {
            m_context.ActionLogs.Add(log);
        }

        public bool Save()
        {
            try
            {
                return m_context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                // TODO: log this!
                return false;
            }
        }
    }
}
