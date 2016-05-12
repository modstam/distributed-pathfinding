using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation
{
    class Agent
    {
        public int id;
        public int x;
        public int y;
        public int goalX;
        public int goalY;

        private bool hasPath = false;

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
            hasPath = true;
        }

        public List<Node> getPath()
        {
            return pathToGoal;
        }

    }
}
