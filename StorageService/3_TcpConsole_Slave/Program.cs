using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageServiceLibrary;
using System.Configuration;
using NLog;
using System.IO;
using System.Net;

namespace TcpConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string ipSlave = ConfigurationManager.AppSettings["IP_Slave"];
            string portSlave = ConfigurationManager.AppSettings["Port_Slave"];

            ISlaveService<User> slave = new StorageService<User>();

            SlaveNetworkService<User> NetSlave = new SlaveNetworkService<User>(slave, new IPEndPoint(IPAddress.Parse(ipSlave), int.Parse(portSlave)));

            Console.WriteLine("Show users on slave instance:");
            Console.ReadKey();

            foreach (var user in slave.Search(u => true))
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Show users on slave instance, again :");
            Console.ReadKey();
            foreach (var user in slave.Search(u => true))
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Show users on slave instance, and again =) :");
            Console.ReadKey();
            foreach (var user in slave.Search(u => true))
            {
                Console.WriteLine(user);
            }
            Console.ReadKey();
        }
    }



}
