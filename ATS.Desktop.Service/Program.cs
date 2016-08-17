﻿using System.ServiceProcess;

namespace ATS.Desktop.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
           
#if DEBUG
            var mailpayService = new AtsDesktopService();
            mailpayService.OnDebug();
            Thread.Sleep(Timeout.Infinite);
#else
             var servicesToRun = new ServiceBase[] 
            { 
                new AtsDesktopService() 
            };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}
