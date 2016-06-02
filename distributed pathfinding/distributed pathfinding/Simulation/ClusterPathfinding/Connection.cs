using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation.ClusterPathfinding
{
    class Connection
    {
        public List<Node> path;
        

        public Connection()
        {
            path = new List<Node>();
        }

        public Connection(List<Node> path)
        {
            this.path = path;
        }
    }
}
