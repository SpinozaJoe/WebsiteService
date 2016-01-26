using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Logging
{
    public class DatabaseActionLogger : IActionLogger
    {
        private ILogRepository m_logRepository;

        public DatabaseActionLogger(ILogRepository logRepository)
        {
            m_logRepository = logRepository;
        }

        public void writeLog(string applicationName, string origin, string ipAddress, ActionType actionType, string message, List<string> tags)
        {
            ActionLog log = new ActionLog()
            {
                Application = applicationName,
                Origin = origin,
                IpAddress = ipAddress,
                ActionType = actionType,
                Message = message,
                // TODO: This should move into some date handling interface
                TimeStamp = DateTime.UtcNow
            };

            if (tags != null && tags.Count > 0)
            {
                log.Tags = string.Join(";", tags.ToArray());
            }

            m_logRepository.AddActionLog(log);

            m_logRepository.Save();
        }
    }
}
