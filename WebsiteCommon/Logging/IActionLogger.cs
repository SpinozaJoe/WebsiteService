using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Logging
{
    public interface IActionLogger
    {
        void writeLog(string applicationName, string origin, string ipAddress, ActionType actionType,
            string message, List<string> tags);
    }
}
