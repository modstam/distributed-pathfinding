using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation.ClusterPathfinding
{
    class ClusterPathfinding
    {

        ClusterMap clusterMap;

        public ClusterPathfinding(ClusterMap clusterMap)
        {
            this.clusterMap = clusterMap;
        }


        public List<Node> findPath(Node start, Node end)
        {
            Cluster startCluster;
            Cluster endCluster;
            //first find which are our start and endClusters     
            findEndClusters(start, end, out startCluster, out endCluster);

            //if we start and end in the same cluster, check if we can get a path without leaving the cluster
            if(startCluster == endCluster)
            {
                var simplePath = new SimplePathfinding();
                var path = simplePath.SimplePath(clusterMap.getMap(), int.MaxValue, start, end, startCluster);
                if (path != null)
                    return path;
            }



            return null;
        }



        private Dictionary<ExitPoint, List<Node>> findPathsFromStart(Node startNode, Cluster startCluster)
        {

            return null;
        }

        private Dictionary<ExitPoint, List<Node>> findPathsToEnd(Node endNode, Cluster endCluster)
        {

            return null;
        }


        private void findEndClusters(Node start, Node end, out Cluster startCluster, out Cluster endCluster)
        {
            startCluster = null;
            endCluster = null;
            foreach (var cluster in clusterMap.getClusterMap())
            {
                if (cluster.inBounds(start))
                    startCluster = cluster;
                if (cluster.inBounds(end))
                    endCluster = cluster;
                if (startCluster != null && endCluster != null)
                    break; //both have been found
            }
        }

        private List<Cluster> findNeighborClusters(Cluster cluster)
        {
            var neighbors = new List<Cluster>();
            var clusters = clusterMap.getClusterMap();
            Cluster neighborCluster;

            //check up
            neighborCluster = clusterMap.getCluster(cluster.clusterX, cluster.clusterY - 1);
            if (neighborCluster != null)
            {
                neighbors.Add(neighborCluster);
                neighborCluster = null;
            }

            //check down
            neighborCluster = clusterMap.getCluster(cluster.clusterX, cluster.clusterY + 1);
            if (neighborCluster != null)
            {
                neighbors.Add(neighborCluster);
                neighborCluster = null;
            }

            //check right
            neighborCluster = clusterMap.getCluster(cluster.clusterX + 1, cluster.clusterY);
            if (neighborCluster != null)
            {
                neighbors.Add(neighborCluster);
                neighborCluster = null;
            }

            //check left
            neighborCluster = clusterMap.getCluster(cluster.clusterX - 1, cluster.clusterY);
            if (neighborCluster != null)
            {
                neighbors.Add(neighborCluster);
                neighborCluster = null;
            }

            return neighbors;
        }

    }
}
