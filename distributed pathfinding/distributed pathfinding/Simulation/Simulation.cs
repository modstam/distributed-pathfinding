using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace distributed_pathfinding.Simulation
{
    class Simulation
    {
        private int numAgents = 0;
        private Map map;
        private volatile bool shouldRun;

        public Simulation(Map map)
        {
            this.map = map;
        }

        private void spawnAgents()
        {


        }

        private void run()
        {
            while (shouldRun)
            {
                syncMap(map);
            }
            Debug.WriteLine("Simulating stopped...");
        }


        private void syncMap(Map map)
        {
            MapSync.putProducedMap(map);
           // Debug.WriteLine("Produced map");
        }

        public void stop()
        {
            shouldRun = false;       
        }

        public void start()
        {
            shouldRun = true;
            Thread runnerThread = new Thread(run);
            runnerThread.Start();
        }

        public void setNumAgents(int agents)
        {
            stop();
            numAgents = agents;
            start();
        }



    }
}
