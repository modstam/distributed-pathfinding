using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using distributed_pathfinding.Utility;
using System.Web.Script.Serialization;

namespace distributed_pathfinding.Networking
{
    class RemoteWorker
    {
        private string ipAddress;
        private TcpClient socket;
        private volatile bool shouldRun = false;

        public RemoteWorker(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public TcpClient tryConnect(string ipAddress)
        {
            
            socket = new TcpClient();
            socket.Connect(ipAddress, 11111);
            Out.put("Connected to host at: " + ipAddress);
            return socket;
        }

        public void run(TcpClient socket)
        {
            NetworkStream stream = socket.GetStream();

            while (shouldRun)
            {
                Thread.Sleep(1000);
                sendCPUInfo(stream);
            }
        }

        public void sendCPUInfo(NetworkStream stream)
        {
            CPUInfo info = CPUManager.getInfo();
            info.status = "READY";
            var serializer = new JavaScriptSerializer();
            var result = serializer.Serialize(info);
            var streamWriter = new StreamWriter(stream);

            streamWriter.Write(result);
        }

        public void start()
        {
            try
            {
                shouldRun = true;
                run(tryConnect(ipAddress));

            }
            catch(SocketException e)
            {
                Out.put(e.ToString());
            }
        }
        
        public void stop()
        {
            shouldRun = false;
        }

        
    }
}
