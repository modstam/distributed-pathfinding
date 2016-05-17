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
        private Host host;
        private RemoteWorker worker;

        public Network(bool workerMode, string ipAddress)
        {
            this.workerMode = workerMode;
            this.ipAddress = ipAddress;

        }

        public void addCPU()
        {
           // add cpu to delegator

        }

        public void disconnectCPU()
        {
            // remove cpu from delegator and cleanup
        }

        public void start()
        {
            if (workerMode)
            {

            }
            else
            {

            }
        }

        public void stop()
        {

        }
    }

}
