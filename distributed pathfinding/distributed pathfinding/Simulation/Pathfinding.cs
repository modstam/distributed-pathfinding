using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation
{
    class Pathfinding
    {


        public List<Node> SimplePath(Map map, List<Node> nodes, int startX, int startY, int endX, int endY)
        {
            /**
             *	this A* variant was implemented straight off of wikipedias version
             *	https://en.wikipedia.org/wiki/A*_search_algorithm
             *
             *  See wiki for more information
            **/
            if(map.isOpen(startX, startY) || !map.isOpen(endX,endY))
            {
                throw new ArgumentException("Cannot use control points as start or goal");
            }

            Node source = map.getNode(startX, startY);
            Node destination = map.getNode(endX, endY);

            HashSet<Node> closedSet = new HashSet<Node>();
            HashSet<Node> openSet = new HashSet<Node>();
            openSet.Add(map.getNode(startX,startY));
            int count = 1;

            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, float> gScore = new Dictionary<Node, float>();
            Dictionary<Node, float> fScore = new Dictionary<Node, float>();



            //initialize scores with max int values
            for (int i = 0; i < nodes.Count; ++i)
            {
                if (nodes[i].type == NodeType.Wall) continue;
                gScore.Add(nodes[i], float.MaxValue);
                fScore.Add(nodes[i], float.MaxValue);
            }

            gScore[map.getNode(startX, startY)] = 0.0f;
            fScore[map.getNode(startX, startY)] = manhattanDistance(source, destination);


            while (count > 0)
            {
                Node current = FindLowestScore(fScore);
                if (current == destination)
                {
                    //Debug.Log ("found path from " + source + " to " + destination);
                    return ConstructPath(cameFrom, current);
                }


                openSet.Remove(current);
                fScore.Remove(current);
                --count;
                closedSet.Add(current);

                List<Node> neighbours = findNeighbours(current, map, closedSet, openSet); 
                foreach (Node neighbor in neighbours)
                {
                    if (closedSet.Contains(neighbor)) continue;

                    float tentativeGScore = gScore[current] + manhattanDistance(current, neighbor);

                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + manhattanDistance(neighbor, destination);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                            ++count;
                        }
                    }
                }

            }

            return new List<Node>();
        }


        private List<Node> ConstructPath(Dictionary<Node, Node> cameFrom, Node current)
        {
            List<Node> path = new List<Node>();
            path.Add(current);
            while(cameFrom.TryGetValue(cameFrom[current], out current))
            {
                path.Add(current);
            }
            return path;
        }


        private int manhattanDistance(Node start, Node end)
        {
            int distance = Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
            return distance;
        }


        private Node FindLowestScore(Dictionary<Node, float> inMap)
        {
            Node lowestNode = null;
            float lowestValue = float.MaxValue;
            foreach (KeyValuePair<Node, float> kvp in inMap)
            {
                if (kvp.Value < lowestValue)
                {
                    lowestValue = kvp.Value;
                    lowestNode = kvp.Key;
                }
            }
            return lowestNode;
        }

        private List<Node> findNeighbours(Node node, Map map, HashSet<Node> closedSet, HashSet<Node> openSet)
        {
            List<Node> neighbours = new List<Node>();
            int x = node.x;
            int y = node.y;

            if (map.isOpen(x + 1, y))
            {
                neighbours.Add(map.getNode(x + 1, y));
            }
            if (map.isOpen(x - 1, y))
            {
                neighbours.Add(map.getNode(x - 1, y));
            }
            if (map.isOpen(x, y + 1))
            {
                neighbours.Add(map.getNode(x, y + 1));
            }
            if (map.isOpen(x, y - 1))
            {
                neighbours.Add(map.getNode(x, y - 1));
            }
            if (map.isOpen(x - 1, y + 1))
            {
                neighbours.Add(map.getNode(x - 1, y + 1));
            }
            if (map.isOpen(x + 1, y + 1))
            {
                neighbours.Add(map.getNode(x + 1, y + 1));
            }
            if (map.isOpen(x + 1, y - 1))
            {
                neighbours.Add(map.getNode(x + 1, y - 1));
            }
            if (map.isOpen(x - 1, y - 1))
            {
                neighbours.Add(map.getNode(x - 1, y - 1));
            }
            closedSet.Add(node);
            openSet.Remove(node);

            return neighbours;
        }




    }
}
