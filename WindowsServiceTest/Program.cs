using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WindowsServiceTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
#if DEBUG

            WorkerTimeChecker myServ = new WorkerTimeChecker();
            myServ.OnDebug();
            System.Threading.Thread.Sleep(10000);



#else

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new WorkerTimeChecker() 
			};
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
