using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using distributed_pathfinding.Utility;

namespace distributed_pathfinding.Simulation
{
    class Simulation
    {
        private int numAgents = 1000;
        private int calcDepth = int.MaxValue;
        private Map map;
        private volatile bool shouldRun;
        private Random rnd;
        private bool firstRun = true;
        private bool simplePath;
        private int clusterSize = 40;

        public Simulation(Map map, bool simplePath)
        {
            this.map = map;
            this.rnd = new Random();
            this.simplePath = simplePath;
        }

        /**
         * This is the core function of the simulation
         * */
        private void run()
        {
            if (simplePath)
            {
                simplePathfinding();
            }
            else
            {
                clusterPathFinding();
            }
        }

        private void clusterPathFinding()
        {
            spawnAgents();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (shouldRun)
            {
                    //syncMap(map);
                    //moveAgents();
                    //Thread.Sleep(100);
            }

            Out.put("Simulating stopped...");
        }

        private void simplePathfinding()
        {
            spawnAgents();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (shouldRun)
            {
                if (firstRun)
                {
                    firstPass();
                }
                else
                {
                    syncMap(map);
                    moveAgents();
                    Thread.Sleep(100);
                }
            }

            Out.put("Simulating stopped...");
        }

        private void moveAgents()
        {
            Dictionary<int, Agent> agents = map.getAgents();
            List<Agent> agentList = agents.Values.ToList();
            int i = 0;
            while (shouldRun && i < agentList.Count)
            {
                Agent agent = agentList[i];

                if (reachedGoal(agent))
                {
                    generateGoal(agent);
                    calculatePath(agent);
                }
                if (agent.getPath() == null || agent.getPath().Count <= 0) //if the agent has no path, lets calculate one 
                {
                    calculatePath(agent);
                }
                takeStep(agent);

                ++i;
            }
        }

        private void calculatePath(Agent agent)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SimplePathfinding aStar = new SimplePathfinding(); 
            List<Node> path = aStar.SimplePath(map, calcDepth, agent.x, agent.y, agent.goalX, agent.goalY);
           // Debug.WriteLine(sw.ElapsedMilliseconds + "ms: " + "Calculated path for agent: " + agent.id + " length: " + path.Count);
            agent.setPath(path);
            sw.Stop();
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
               // Debug.WriteLine("added agent at " + x + ", " + y + " with goal at "+ map.getAgent(i).goalX + ", " + map.getAgent(i).goalY);
            }
            Out.put("agents added");
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
            if (shouldRun)
            {
                Out.put("Stopping simulation...");
            }              
            shouldRun = false;       
        }

        public void start()
        {
            shouldRun = true;
            firstRun = true;
            Out.put("Starting simulation...");

            Thread runnerThread = new Thread(run);
            runnerThread.Start();
        }

        public void setNumAgents(int agents)
        {
            numAgents = agents;

        }

        public int getNumAgents()
        {
            return numAgents;
        }

        private void firstPass()
        {
            firstRun = false;
            int numThreads = 10;
            int increment = numAgents / numThreads +1;
            int start = 0;
            int end = increment;
            List<Thread> threads = new List<Thread>();

            for(int i = 0; i < numThreads; ++i)
            {
                CalcRange range = new CalcRange();
                range.start = start;
                range.end = end;
                start = range.end;
                end += increment;

                Out.put("Thread " + i + " range: " + range.start + "-" + range.end); 

                Thread thread = new Thread(calcPathForAgents);
                thread.Start(range);
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        private void calcPathForAgents(object range)
        {
            var agentRange = (CalcRange)range;
            if (agentRange.end >= numAgents - 1) agentRange.end = numAgents - 1;

            Dictionary<int, Agent> agents = map.getAgents();
            List<Agent> agentList = agents.Values.ToList();
            int i = agentRange.start;
            while(shouldRun && i < agentRange.end)
            {
                calculatePath(agentList[i]);
                ++i;
            }

        }

        struct CalcRange
        {
            public int start;
            public int end;
        }

        public Map getMapCopy()
        {
            return new Map(this.map);
        }

        public void setClusterSize(int clusterSize)
        {
            this.clusterSize = clusterSize;
        }



    }
}
