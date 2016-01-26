using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Logging
{
    public enum ActionType
    {
        WebAction,
        SendEmail
    }

    public class ActionLog
    {
        public long Id { get; set; }
        public string Application { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Origin { get; set; }
        public string IpAddress { get; set; }
        public ActionType ActionType { get; set; }
        public string Message { get; set; }
        public string Tags { get; set; }
    }
}
