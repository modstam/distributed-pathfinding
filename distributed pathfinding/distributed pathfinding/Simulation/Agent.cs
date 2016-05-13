using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace distributed_pathfinding.Simulation
{
    class Agent
    {
        public int id;
        public int x;
        public int y;
        public int goalX;
        public int goalY;

        private bool aquiredPath = false;

        public List<Node> pathToGoal;



        public Agent(int id,int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public void move(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void setGoal(int x, int y)
        {
            this.goalX = x;
            this.goalY = y;
        }

        public void setPath(List<Node> newPath)
        {
            pathToGoal = newPath;
            aquiredPath = true;
        }

        public List<Node> getPath()
        {
            return pathToGoal;
        }

        public bool hasPath()
        {
            return aquiredPath;
        }

        public Agent getShallowCopy()
        {
            Agent newAgent = new Agent(id, x, y);
            newAgent.goalX = this.goalX;
            newAgent.goalY = this.goalY;

            return newAgent;
        }

    }
}
