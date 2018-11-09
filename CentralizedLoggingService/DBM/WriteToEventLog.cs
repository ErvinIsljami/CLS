using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    public sealed class WriteToEventLog
    {
        private static EventLog newLog;
        private static string LogName = "Events";
        private static string SourceName = ".AABoske";
        private static WriteToEventLog instance = null;

        static WriteToEventLog()
        {
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
            newLog = new EventLog(LogName, Environment.MachineName, SourceName);

        }
        public static WriteToEventLog Instance()
        {
            if(instance == null)
            {
                instance = new WriteToEventLog();
            }
            return instance;
        }

        public void LogSuccess(string user, string method)
        {
            newLog.WriteEntry(string.Format("User {0} successfully accessed to {1}.", user, method));
        }
        public void LogFailure(string user, string method, string reason)
        {
            newLog.WriteEntry(string.Format("User {0} failed to access {1}. Reason: {2}.", user, method, reason));
        }

    }
}
