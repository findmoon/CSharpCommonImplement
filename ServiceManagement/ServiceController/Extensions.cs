using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ServiceProcess
{
    public static class Extensions
    {
        public static bool Exists(this ServiceController[] services,string serviceName)
        {
            foreach (var service in services)
            {
                if (service.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
