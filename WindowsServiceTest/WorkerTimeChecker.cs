using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace WindowsServiceTest
{
    public partial class WorkerTimeChecker : ServiceBase
    {
        static DateTime switchOn;
        static DateTime switchOff;
        static string fileName;
        static string lineHeader;
        static string[] mounths = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        static FileStream fstream;
        public WorkerTimeChecker()
        {
            InitializeComponent();
            //Microsoft.Win32.SystemEvents.SessionEnded += new Microsoft.Win32.SessionEndedEventHandler(SystemEvents_SessionEnded);
        }


        void SystemEvents_SessionEnded(object sender, Microsoft.Win32.SessionEndedEventArgs e)
        {
            //File.Create(AppDomain.CurrentDomain.BaseDirectory + "SystemEvents_SessionEnded.txt");
            AddSwitchOffStr();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            AddSwitchOnStr();
        }

        protected override void OnStop()
        {
            //File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");
            AddSwitchOffStr();
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            //File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnShutdown.txt");
            AddSwitchOffStr();
            base.OnShutdown();
        }

        private void AddSwitchOffStr()
        {
            try
            {
                switchOff = DateTime.Now;
                fileName = AppDomain.CurrentDomain.BaseDirectory + mounths[switchOff.Month - 1] + " " + switchOff.Year.ToString() + ".txt";
                lineHeader = switchOff.Day.ToString("D2") + "." + switchOff.Month.ToString("D2") + "." + switchOff.Year.ToString();
                fstream = File.Open(fileName, FileMode.Append, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fstream))
                {
                    sw.WriteLine(lineHeader + ": " + Environment.UserDomainName + " " + Environment.UserName + " Off: " + switchOff.ToShortTimeString());
                }
            }
            catch (System.Exception ex)
            {
                File.AppendAllLines(AppDomain.CurrentDomain.BaseDirectory + "errors.txt", new string[] { ex.StackTrace, ex.Message });
            }
        }

        private void AddSwitchOnStr()
        {
            switchOn = DateTime.Now;
            fileName = AppDomain.CurrentDomain.BaseDirectory + mounths[switchOn.Month - 1] + " " + switchOn.Year.ToString() + ".txt";
            fstream = File.Open(fileName, FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fstream))
            {
                lineHeader = switchOn.Day.ToString("D2") + "." + switchOn.Month.ToString("D2") + "." + switchOn.Year.ToString();
                sw.WriteLine(lineHeader + ": " + Environment.UserDomainName + " " + Environment.UserName + " On: " + switchOn.ToShortTimeString());
            }
        }
    }
}
