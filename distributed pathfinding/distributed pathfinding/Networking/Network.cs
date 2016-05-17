using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using distributed_pathfinding.Simulation;

namespace distributed_pathfinding.Networking
{
    class Network
    {
        private bool workerMode;
        private string ipAddress;
        private int port = 12345;
        private Host host;
        private RemoteWorker worker;

        public Network(bool workerMode, string ipAddress)
        {
            this.workerMode = workerMode;
            this.ipAddress = ipAddress;
            worker = new RemoteWorker(ipAddress);
            host = new Host(port);

        }

        public Network(bool workerMode)
        {
            this.workerMode = workerMode;
            this.ipAddress = "127.0.0.1";
            worker = new RemoteWorker(ipAddress);
            host = new Host(port);
        }

        public void start()
        {
            if (workerMode)
            {
                worker = new RemoteWorker(ipAddress);
                worker.start();
            }
            else
            {
                host = new Host(port);
                host.start();
            }
        }

        public void stop()
        {
            if (workerMode)
            {
                worker.stop();
            }
            else
            {
                host.stop();
            }
        }

        public void setWorkerMode(bool mode)
        {
            if(workerMode != mode)
            {
                stop();
                workerMode = mode;
            }
          
            
        }
    }

}