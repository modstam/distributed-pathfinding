using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace distributed_pathfinding.Simulation
{
    class Pathfinding
    {


        public List<Node> SimplePath(Map map,int depth, int startX, int startY, int endX, int endY)
        {
            /**
             *	this A* variant was implemented straight off of wikipedias version
             *	https://en.wikipedia.org/wiki/A*_search_algorithm
             *
             *  See wiki for more information
            **/

            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            if (!map.isOpen(endX,endY))
            {
                throw new ArgumentException("Start and end nodes must be empty");
            }
            List<Node> nodes = map.getNodes();
            int source = map.getNode(startX, startY).id;
            int destination = map.getNode(endX, endY).id;

            HashSet<int> closedSet = new HashSet<int>();
            HashSet<int> openSet = new HashSet<int>();
            openSet.Add(map.getNode(startX,startY).id);

            Dictionary<int, int> cameFrom = new Dictionary<int, int>();
            Dictionary<int, float> gScore = new Dictionary<int, float>();
            Dictionary<int, float> fScore = new Dictionary<int, float>();


            gScore[map.getNode(startX, startY).id] = 0.0f;
            fScore[map.getNode(startX, startY).id] = manhattanDistance(map, source, destination);
            int curDepth = 0;
            while (openSet.Count > 0)
            {
             
                int current = FindLowestScore(fScore);
                if (current == destination || curDepth >= depth-1)
                {
                    //sw.Stop();
                    //Debug.WriteLine("Elapsed={0}", sw.Elapsed);
                    return ConstructPath(map, cameFrom, current);                  
                }


                openSet.Remove(current);
                fScore.Remove(current);
                closedSet.Add(current);

                List<int> neighbours = findNeighbours(current, map, closedSet, openSet); 
                foreach (int neighbor in neighbours)
                {
                    if (closedSet.Contains(neighbor)) continue;

                    float tentativeGScore = gScore[current] + 1;


                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if(tentativeGScore >= gScore[ neighbor])
                    {
                        continue;
                    }
            
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + manhattanDistance(map, neighbor, destination);      
                }
                ++curDepth;
            }
            //sw.Stop();
            //Debug.WriteLine("Elapsed={0}", sw.Elapsed);
            return null; //failure
        }


        private List<Node> ConstructPath(Map map, Dictionary<int, int> cameFrom, int current)
        {
            LinkedList<Node> path = new LinkedList<Node>();
            path.AddLast(map.getNode(current));
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.AddFirst(map.getNode(current));
            }
            return path.ToList();
        }


        private int manhattanDistance(Map map, int startID, int endID)
        {
            Node start = map.getNode(startID);
            Node end = map.getNode(endID);
            int distance = Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
            return distance;
        }


        private int FindLowestScore(Dictionary<int, float> inMap)
        {
            int lowestNode = 0;
            float lowestValue = float.MaxValue;
            foreach (KeyValuePair<int, float> kvp in inMap)
            {
                if (kvp.Value < lowestValue)
                {
                    lowestValue = kvp.Value;
                    lowestNode = kvp.Key;
                }
            }
            return lowestNode;
        }

        private List<int> findNeighbours(int node, Map map, HashSet<int> closedSet, HashSet<int> openSet)
        {
            List<int> neighbours = new List<int>();
            int x = map.getNode(node).x;
            int y = map.getNode(node).y;

            if (map.isOpen(x + 1, y))
            {
                neighbours.Add(map.getNode(x + 1, y).id);
            }
            if (map.isOpen(x - 1, y))
            {
                neighbours.Add(map.getNode(x - 1, y).id);
            }
            if (map.isOpen(x, y + 1))
            {
                neighbours.Add(map.getNode(x, y + 1).id);
            }
            if (map.isOpen(x, y - 1))
            {
                neighbours.Add(map.getNode(x, y - 1).id);
            }
            if (map.isOpen(x - 1, y + 1))
            {
                neighbours.Add(map.getNode(x - 1, y + 1).id);
            }
            if (map.isOpen(x + 1, y + 1))
            {
                neighbours.Add(map.getNode(x + 1, y + 1).id);
            }
            if (map.isOpen(x + 1, y - 1))
            {
                neighbours.Add(map.getNode(x + 1, y - 1).id);
            }
            if (map.isOpen(x - 1, y - 1))
            {
                neighbours.Add(map.getNode(x - 1, y - 1).id);
            }
            closedSet.Add(node);
            openSet.Remove(node);

            return neighbours;
        }




    }
}
