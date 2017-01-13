using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    public partial class ClientService : ServiceBase
    {
        public ClientService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = "Client";
            string logName = "ClientNewLog";
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if(args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Client"))
            {
                System.Diagnostics.EventLog.CreateEventSource("Client", "ClientNewLog");
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");

            //  Set up a timer to trigger every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            //  TODO: Insert monitoring activities here.
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, 210001);
        }
    }
}
