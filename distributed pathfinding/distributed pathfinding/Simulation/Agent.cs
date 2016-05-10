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

    }
}
