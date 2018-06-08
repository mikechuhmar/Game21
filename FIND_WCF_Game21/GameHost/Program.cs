using System;
using System.Linq;
using System.ServiceModel;
using System.Net;


namespace GameHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string myHost = Dns.GetHostName();
            string myIP = Dns.GetHostByName(myHost).AddressList.Last().ToString();
            
            Uri http = new Uri("http://" + myIP + ":8301/");
            Uri netTcp = new Uri("net.tcp://" + myIP + ":8302/");
            
            using (var host = new ServiceHost(typeof(WCF_Game21.ServiceGame), http, netTcp))
            {
                host.Open();
                
                Console.WriteLine("Хост запущен");
                Console.ReadLine();
                
                //host.ChannelDispatchers.First().Close();
               
                host.Close();
            }
        }
    }
}
