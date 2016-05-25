using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation.ClusterPathfinding
{
    class Cluster
    {
        public int width;
        public int height;
        public int top;
        public int left;
        public int clusterX;
        public int clusterY;

        public List<ExitPoint> exits;



        public Cluster(int top, int left, int width, int height, int clusterX, int clusterY)
        {
            this.width = width;
            this.height = height;
            this.top = top;
            this.left = left;
            this.clusterX = clusterX;
            this.clusterY = clusterY;

            exits = new List<ExitPoint>();
        }

        public void setExits(List<ExitPoint> exits)
        {
            this.exits = exits;
        }

        public void addExit(int x, int y)
        {
            if(!hasExit(x,y))
                exits.Add(new ExitPoint(x, y));
        }

        public bool hasExit(int x, int y)
        {
            foreach(ExitPoint exit in exits)
            {
                if (x == exit.x && y == exit.y)
                    return true;
            }
            return false;
        }

    }
}
