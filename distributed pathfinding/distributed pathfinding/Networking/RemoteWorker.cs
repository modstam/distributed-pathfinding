using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using distributed_pathfinding.Utility;
using System.Web.Script.Serialization;

namespace distributed_pathfinding.Networking
{
    class RemoteWorker
    {
        private string ipAddress;
        private int port;
        private TcpClient socket;
        private volatile bool shouldRun = false;

        public RemoteWorker(string ipAddress,int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void tryConnect(object ipAddress)
        {
            try
            {
            string ip = (string)ipAddress;
            socket = new TcpClient();
            socket.Connect(ip, port);
            Out.put("Connected to host at: " + ipAddress);
            run(socket);

            }
            catch (SocketException e)
            {
                Out.put(e.ToString());
            }
        }

        public void run(TcpClient socket)
        {
            NetworkStream stream = socket.GetStream();
            Stopwatch sw = new Stopwatch();

            while (shouldRun)
            {
                sw.Start();
                

                if(sw.ElapsedMilliseconds > 5000)
                {
                    sendCPUInfo(stream);
                }             
                sw.Reset();
            }
            stream.Dispose();
            socket.Close();
        }

        public void sendCPUInfo(NetworkStream stream)
        {
            CPUInfo info = CPUManager.getInfo();
            info.status = "READY";
            var serializer = new JavaScriptSerializer();
            var result = serializer.Serialize(info);
            var streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine(result);
            streamWriter.Flush();
            Out.put("Sent CPUinfo to host");
        }

        public void start()
        {
            shouldRun = true;
            Thread thread = new Thread(tryConnect);
            thread.Start(ipAddress);
        }
        
        public void stop()
        {
            shouldRun = false;
        }

        
    }
}
