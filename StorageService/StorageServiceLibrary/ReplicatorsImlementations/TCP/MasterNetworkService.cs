using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NLog;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using StorageServiceLibrary;

namespace StorageServiceLibrary
{
    public class MasterNetworkService<T> : IDisposable
    {
        IMasterService<T> master;
        List<TcpClient> slaveClients;
        BinaryFormatter formatter;

        public MasterNetworkService(IMasterService<T> master, params IPEndPoint[] slaveEndPoints)
        {
            if (master == null) throw new ArgumentNullException(nameof(master));
            if (slaveEndPoints == null) throw new ArgumentNullException(nameof(slaveEndPoints));

            this.formatter = new BinaryFormatter();
            this.master = master;
            this.slaveClients = new List<TcpClient>();


            foreach (IPEndPoint slaveIPendPoint in slaveEndPoints)
            {

                slaveClients.Add(new TcpClient(slaveIPendPoint.Address.ToString(), slaveIPendPoint.Port));
            }

            this.master.changeEvent += MasterEventHandler;
            Initite();
        }

        private void MasterEventHandler(object sender, ChangeMessageEventArgs<T> e)
        {

            foreach (TcpClient slave in slaveClients)
            {

                try
                {
                    NetworkStream stream = slave.GetStream();
                    formatter.Serialize(stream, e);
                    stream.Close();
                    slave.Close();
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Trace(ex);
                }
                finally
                {
                    slave.Close();
                }
            }
        }

        private void Initite()
        {

            foreach (TcpClient slave in slaveClients)
            {
                try
                {

                    NetworkStream stream = slave.GetStream();
                    formatter.Serialize(stream, new ChangeMessageEventArgs<T>(Actions.Add, master.Search(u => true).ToArray()));
                    stream.Close();
                    slave.Close();

                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Trace(ex);
                }
                finally
                {
                    slave.Close();
                }

            }

        }


        public void Dispose()
        {
            foreach (var client in slaveClients)
            {
                if (client != null)
                    client.Close();
            }
        }
    }



}
