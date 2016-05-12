using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation
{
    class Node
    {
        public int id;
        public int x;
        public int y;
        public NodeType type;
        public Agent agent = null;     

        public Node(int id, int x, int y, NodeType type)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.type = type;
        }



    }

    enum NodeType { Wall, Empty, Agent };
}
