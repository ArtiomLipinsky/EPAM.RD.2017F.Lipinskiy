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
            string recoveryFilePath = ConfigurationManager.AppSettings["RecoveryFilePath"];
            string ipSlave1 = ConfigurationManager.AppSettings["IP_Slave1"];
            string portSlave1 = ConfigurationManager.AppSettings["Port_Slave1"];
            string ipSlave2 = ConfigurationManager.AppSettings["IP_Slave2"];
            string portSlave2 = ConfigurationManager.AppSettings["Port_Slave2"];
            string ipSlave3 = ConfigurationManager.AppSettings["IP_Slave3"];
            string portSlave3 = ConfigurationManager.AppSettings["Port_Slave3"];

            var userList = new List<User> {
            new User() { FirstName = "Donald", LastName = "Tramp", Age = 71 },
            new User() { FirstName = "Pik", LastName = "Loas", Age = 15 },
            new User() { FirstName = "Lisa", LastName = "Poret", Age = 7 },
            new User() { FirstName = "Garry", LastName = "Fitas", Age = 33 },
            new User() { FirstName = "Genadiy", LastName = "Poh", Age = 21 },
            new User() { FirstName = "Omag", LastName = "Ram", Age = 25, Id = 133 }
        };

            IMasterService<User> master = new StorageService<User>();

            if (File.Exists(recoveryFilePath))
            {
                master.RecoverFromFile(recoveryFilePath);
            }

            else
            {
                foreach (User user in userList)
                {
                    master.Add(user);
                }
            }

            MasterNetworkService<User> NetMaster = new MasterNetworkService<User>(master,
                new IPEndPoint[]{
                new IPEndPoint(IPAddress.Parse(ipSlave1), int.Parse(portSlave1)),
                   //new IPEndPoint(IPAddress.Parse(ipSlave2), int.Parse(portSlave2)),
                      //new IPEndPoint(IPAddress.Parse(ipSlave3), int.Parse(portSlave3))

            });

            Console.WriteLine("Press any key. This will add User: Slave TCP 1");
            Console.ReadKey();

            master.Add(new User() { FirstName = "Slave", LastName = "TCP", Age = 1 });

            Console.WriteLine("Done");

            Console.WriteLine("All user in master instance:");

            foreach (var user in master.Search(u => true))
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Press any key. This will add User: TCP Slave 2");
            Console.ReadKey();

            master.Add(new User() { FirstName = "TCP", LastName = "Slave", Age = 2 });

            Console.WriteLine("Done");

            Console.WriteLine("All user in master instance:");
            Console.ReadKey();

            foreach (var user in master.Search(u => true))
            {
                Console.WriteLine(user);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }
    }



}
