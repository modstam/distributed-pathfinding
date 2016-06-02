using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using distributed_pathfinding.Utility;

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
        public string id;

        public List<ExitPoint> exits;
        private Dictionary<ExitPoint, Dictionary<ExitPoint, List<Node>>> paths;



        public Cluster(int top, int left, int width, int height, int clusterX, int clusterY)
        {
            this.width = width;
            this.height = height;
            this.top = top;
            this.left = left;
            this.clusterX = clusterX;
            this.clusterY = clusterY;
            this.id = left + "," + top;

            paths = new Dictionary<ExitPoint, Dictionary<ExitPoint, List<Node>>>();
            exits = new List<ExitPoint>();
        }

        public void setExits(List<ExitPoint> exits)
        {
            this.exits = exits;
        }

        public void addPath(ExitPoint exit1, ExitPoint exit2, List<Node> path)
        {
            if (!paths.ContainsKey(exit1)) paths[exit1] = new Dictionary<ExitPoint, List<Node>>();
            if (!paths.ContainsKey(exit2)) paths[exit2] = new Dictionary<ExitPoint, List<Node>>();
            paths[exit1][exit2] = path;
            paths[exit2][exit1] = reversePath(path);   
        }

        public List<Node> getPath(ExitPoint exit1, ExitPoint exit2)
        {
            if (!paths.ContainsKey(exit1) || !paths[exit1].ContainsKey(exit2))
                return new List<Node>();
            return paths[exit1][exit2];
        }

        public Dictionary<ExitPoint, List<Node>> getPaths(ExitPoint exit)
        {
            if (paths.ContainsKey(exit)) return paths[exit];
            return null;
        }

        public void addExit(ExitPoint exit)
        {
            if(!hasExit(exit.x,exit.y))
                exits.Add(exit);
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


        public bool inBounds(int x, int y)
        {
            if ((x >= left && x < left + width) && (y >= top && y < top + height))
                return true;
            return false;
        }

        public bool inBounds(Node node)
        {
            if ((node.x >= left && node.x < left + width) && (node.y >= top && node.y < top + height))
                return true;
            return false;
        }

        private List<Node> reversePath(List<Node> path)
        {
            var reverse = new Node[path.Count];
            for(int i = 0; i < path.Count; ++i)
            {
                reverse[reverse.Length - 1 - i] = path[i];
            }
            return reverse.ToList();
        }

        public void connectExits(Map map)
        {
            var closed = new Dictionary<string, HashSet<string>>();
            var pathfinding = new SimplePathfinding();
            int numPaths = 0;

            foreach(ExitPoint exit1 in exits)
            {
                foreach(ExitPoint exit2 in exits)
                {
                    if (exit1.x==exit2.x && exit1.y == exit2.y) continue;
                    if (closed.ContainsKey(exit1.id) && closed[exit1.id].Contains(exit2.id)) continue;
                    if (closed.ContainsKey(exit2.id) && closed[exit2.id].Contains(exit1.id)) continue;

                    var path = pathfinding.SimplePath(map, int.MaxValue, exit1.x, exit1.y, exit2.x, exit2.y, this);
                    if(path != null)
                    {
                        if (!closed.ContainsKey(exit1.id)) closed[exit1.id] = new HashSet<string>();
                        if (!closed.ContainsKey(exit2.id)) closed[exit2.id] = new HashSet<string>();
                        closed[exit1.id].Add(exit2.id);
                        closed[exit2.id].Add(exit1.id);
                        addPath(exit1, exit2, path);                        
                        ++numPaths;
                    }             
                }
            }
           Out.put("Cluster " + id + ": paths count : " + numPaths);
            //foreach (ExitPoint exit1 in exits)
            //{
            //    Out.put(exit1.id);
            //}

        }

        public List<ExitPoint> isConnected(Cluster otherCluster)
        {
            var connectionPoints = new List<ExitPoint>();
            foreach(ExitPoint exit in exits)
            {
                if (otherCluster.inBounds(exit.connection.x, exit.connection.y))
                    connectionPoints.Add(exit);
            }

            return connectionPoints;
        } 

    }
}
