using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using distributed_pathfinding.Utility;
using System.Web.Script.Serialization;
using distributed_pathfinding.Simulation;

namespace distributed_pathfinding.Networking
{
    class Host
    {

        private volatile bool shouldRun = false;
        private Dictionary<int, Thread> connectedClients;

        public Host()
        { 

        }


        private void run(object client)
        {
            try
            {
                var con = (TcpClient)client;
                var stream = con.GetStream();
                var streamReader = new StreamReader(stream);
                string line = "";

                while (shouldRun)
                {

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Out.put(con.Client.RemoteEndPoint.ToString() + " says: " + line);
                        var JSONObject = readJSON(line);
                        decideAction(JSONObject);
                    }            
                }
            }
            catch (SocketException e)
            {
                Out.put(e.ToString());
            }

        }

        private void listen()
        {
            try
            {
                int connections = 0;

                Int32 port = 11111;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                TcpListener connectionListener = new TcpListener(localAddr, port);
                connectionListener.Start();

                while (shouldRun)
                {
                    Out.put("Host is listening for connections...");

                    TcpClient client = connectionListener.AcceptTcpClient();
                    Console.WriteLine(client.Client.RemoteEndPoint.ToString() + "connected!");


                    Thread thread = new Thread(run);
                    thread.Start(client);
                    connectedClients[connections] = thread;

                    connections++;

                }
            }
            catch(SocketException e)
            {
                Out.put(e.ToString());
            }           
        }

        private Object readJSON(string input)
        {
            if ((input.StartsWith("{") && input.EndsWith("}")) || //For object
                (input.StartsWith("[") && input.EndsWith("]"))) //For array
            {
                var serializer = new JavaScriptSerializer();
                var deserializedResult = serializer.Deserialize<CPUInfo>(input);
                Out.put("Found new remote CPU");
                return deserializedResult;
            }
            else
            {
                Out.put("Tried reading something that wasn't JSON..");
                return null;
            }
        }

        private void decideAction(object input)
        {
            if (input == null) return;
            if(input is CPUInfo)
            {
                SystemDelegator.addCPU((CPUInfo) input);
            }
        }

        public void start()
        {
            shouldRun = true;
            listen();
        }

        public void stop()
        {
            shouldRun = false;
        }




    }
}
