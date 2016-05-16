using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation
{
    class Networking
    {
        private SystemDelegator delegator;
        private bool serverMode;
        private string ipAddress;

        public Networking(bool serverMode, string ipAddress)
        {
            this.serverMode = serverMode;
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
    }

    enum NetworkMode {Client, Server};
}
