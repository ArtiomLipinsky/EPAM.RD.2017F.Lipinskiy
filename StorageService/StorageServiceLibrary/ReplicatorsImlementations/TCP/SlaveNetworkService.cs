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

    public class SlaveNetworkService<T> : IDisposable
    {
        ISlaveService<T> slave;

        TcpListener masterListner;
        TcpClient connection;
        BinaryFormatter formatter;


        public SlaveNetworkService(ISlaveService<T> slave, IPEndPoint masterEndPoint)
        {
            if (masterEndPoint == null) throw new ArgumentNullException(nameof(masterEndPoint));
            if (slave == null) throw new ArgumentNullException(nameof(slave));
            this.slave = slave;
            connection = null;
            this.masterListner = new TcpListener(masterEndPoint.Address, masterEndPoint.Port);
            this.formatter = new BinaryFormatter();
            Start();
        }

        private async void Start()
        {
            try
            {
                masterListner.Start();

                while (true)
                {
                    connection = await masterListner.AcceptTcpClientAsync();
                    NetworkStream stream = connection.GetStream();
                    ChangeMessageEventArgs<T> message = (ChangeMessageEventArgs<T>)formatter.Deserialize(stream);
                    slave.MasterChangeEvent(this, message);
                    stream.Close();
                    connection.Close();
                }
            }
            catch (SocketException ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Trace(ex.Message);
            }
            finally
            {
                masterListner.Stop();
            }


        }

        public void Dispose()
        {
            if (connection != null)
                connection.Close();
            if (masterListner != null)
                masterListner.Stop();
        }
    }
}
