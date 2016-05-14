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
        private int numAgents = 1000;
        private int calcDepth = int.MaxValue;
        private Map map;
        private volatile bool shouldRun;
        private Random rnd;

        public Simulation(Map map)
        {
            this.map = map;
            this.rnd = new Random();
        }

        /**
         * This is the core function of the simulation
         * */
        private void run()
        {
            spawnAgents();

            while (shouldRun)
            {
                syncMap(map);
                moveAgents();
                Thread.Sleep(100);
            }
            Debug.WriteLine("Simulating stopped...");
        }

        private void moveAgents()
        {
            Stopwatch sw = new Stopwatch();
            Dictionary<int,Agent> agents = map.getAgents();
            foreach(Agent agent in agents.Values.ToList())
            {
                sw.Start();

                if (reachedGoal(agent)) 
                {
                    generateGoal(agent);
                    Debug.WriteLine("new goal is: " + agent.goalX + ", " + agent.goalY + " agent id: " + agent.id);
                    calculatePath(agent);
                }
                if(agent.getPath() == null || agent.getPath().Count <= 0) //if the agent has no path, lets calculate one 
                {                   
                    calculatePath(agent);
                }
                takeStep(agent);

                sw.Stop();
            }
        }

        private void calculatePath(Agent agent)
        {
            Pathfinding aStar = new Pathfinding(); 
            List<Node> path = aStar.SimplePath(map, calcDepth, agent.x, agent.y, agent.goalX, agent.goalY);
            //Debug.WriteLine("Calculated path for agent: " + agent.id + " length: " + path.Count);
            agent.setPath(path);
        }

        private void takeStep(Agent agent)
        {
            if(agent.getPath() != null)
            {
                int x = agent.getPath()[0].x;
                int y = agent.getPath()[0].y;
                agent.getPath().RemoveAt(0);

                //Debug.WriteLine("Moved agent: "+ agent.id + " from ["+ agent.x + "," + agent.y + "] to " + "["+ x + ", " + y + "]");
                map.moveAgent(agent.id, x, y);
            }

        }

        private bool reachedGoal(Agent agent)
        {
            if (agent.x == agent.goalX && agent.y == agent.goalY)
            {
                //Debug.WriteLine("Agent " + agent.id + " reached the goal");
                return true; 
            }
            return false;
        }

        private void spawnAgents()
        {
            //rnd = new Random();
            for (int i = 0; i < numAgents; ++i)
            {
                int x = rnd.Next(0, map.getMatrixRowSize());
                int y = rnd.Next(0, map.getMatrixColumnSize());

                while (map.getNode(x, y).type != NodeType.Empty)
                {
                    x = rnd.Next(0, map.getMatrixRowSize());
                    y = rnd.Next(0, map.getMatrixColumnSize());
                }

                map.addAgent(i, x, y);
                generateGoal(map.getAgent(i));
                Debug.WriteLine("added agent at " + x + ", " + y + " with goal at "+ map.getAgent(i).goalX + ", " + map.getAgent(i).goalY);
            }
        }

        private void generateGoal(Agent agent)
        {
            //rnd = new Random();

            int x = rnd.Next(0, map.getMatrixRowSize());
            int y = rnd.Next(0, map.getMatrixColumnSize());
            NodeType type = map.getNode(x, y).type;
            while (type != NodeType.Empty)
            {
               
                x = rnd.Next(0, map.getMatrixRowSize());
                y = rnd.Next(0, map.getMatrixColumnSize());
                type = map.getNode(x, y).type;
            }

            agent.setGoal(x, y);
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
