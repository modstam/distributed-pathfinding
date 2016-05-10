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
            Random rnd = new Random();
            for(int i = 0; i < numAgents; ++i)
            {
               int x = rnd.Next(0, map.getMatrixRowSize());
               int y = rnd.Next(0, map.getMatrixColumnSize());

                while (map.getNode(x,y).type != NodeType.Empty)
                {
                    x = rnd.Next(0, map.getMatrixRowSize());
                    y = rnd.Next(0, map.getMatrixColumnSize());
                }

                map.addAgent(i, x, y);
            }

        }

        private void generateGoal(Node node)
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
            MapSync.putProducedMap(map.getNodes());
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
