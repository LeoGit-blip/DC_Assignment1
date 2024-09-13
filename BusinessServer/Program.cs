using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Business Server Online");
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();
            host = new ServiceHost(typeof(BServer));

            host.AddServiceEndpoint(typeof(BServerInterface), tcp, "net.tcp://localhost:8200/BusinessService");
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
