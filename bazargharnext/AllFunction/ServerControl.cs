using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace bazargharnext.AllFunction
{
    public class ServerControl
    {
        public ServerControl() {
            var mysql = new System.ServiceProcess.ServiceController("mysql");
            var apache= new System.ServiceProcess.ServiceController("apache");
            if (mysql.Status == ServiceControllerStatus.Stopped)
            {
                mysql.Start();
            }
            if (apache.Status == ServiceControllerStatus.Stopped)
            {
                mysql.Start();
            }
        }

}
}
