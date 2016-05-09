using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation
{
    class Actor
    {
        public int id;
        public int x;
        public int y;
        public ActorType type;     

        public Actor(int id, int x, int y, ActorType type)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.type = type;
        }
    }

    enum ActorType { Wall, Agent };
}
