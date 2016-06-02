using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation.ClusterPathfinding
{
    class ClusterMap
    {
        Cluster[,] clusterMap;
        Dictionary<ExitPoint, Dictionary<ExitPoint, Connection>> connections;
        Map map;

        public ClusterMap(Cluster[,] clusters, Map map)
        {
            this.clusterMap = clusters;
            this.map = map;
            this.connections = new Dictionary<ExitPoint, Dictionary<ExitPoint, Connection>>();
        }

        public bool hasConnection(ExitPoint exit1, ExitPoint exit2)
        {
            try
            {
                if (connections[exit1][exit2] != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public Dictionary<ExitPoint, Connection> getConnections(ExitPoint exit)
        {
            if (connections.ContainsKey(exit))
            {
                return connections[exit];
            }
            return null;
        }

        public void addConnection(ExitPoint exit1, ExitPoint exit2, List<Node> path)
        {
            if (!connections.ContainsKey(exit1))
                connections.Add(exit1, new Dictionary<ExitPoint, Connection>());
            if (path == null)
                connections[exit1].Add(exit2, new Connection());
            else
                connections[exit1].Add(exit2, new Connection(path));
        }
        

        public Cluster[,] getClusterMap()
        {
            return this.clusterMap;
        }

        public Map getMap()
        {
            return this.map;
        }

        public Cluster getCluster(int x, int y)
        {
            if (x < 0 || y < 0) return null;
            if (x >= clusterMap.GetLength(0) || y >= clusterMap.GetLength(1)) return null;
            else return clusterMap[x, y];
        }
    }
}
