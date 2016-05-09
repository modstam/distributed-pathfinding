using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation
{
    class Pathfinding
    {


        public List<Node> SimplePath(int startX, int startY, int destX, int destY)
        {
            /**
             *	this A* variant was implemented straight off of wikipedias version
             *	https://en.wikipedia.org/wiki/A*_search_algorithm
             *
             *  See wiki for more information
            **/

            if (nodes[source].isControlPoint || nodes[destination].isControlPoint)
            {
                throw new ArgumentException("Cannot use control points as start or goal");
                return new List<Edge>();
            }


            HashSet<int> closedSet = new HashSet<int>();
            HashSet<int> openSet = new HashSet<int>();
            openSet.Add(source);
            int count = 1;

            Dictionary<int, int> cameFrom = new Dictionary<int, int>();
            Dictionary<int, float> gScore = new Dictionary<int, float>();
            Dictionary<int, float> fScore = new Dictionary<int, float>();



            //initialize scores with max int values
            for (int i = 0; i < nodes.Count; ++i)
            {
                if (!nodes[i].isActive) continue;
                gScore.Add(i, float.MaxValue);
                fScore.Add(i, float.MaxValue);
            }

            gScore[source] = 0.0f;
            fScore[source] = Vector3.Distance(nodes[source].pos, nodes[destination].pos);


            while (count > 0)
            {
                int current = FindLowestScore(fScore);
                if (current == destination)
                {
                    //Debug.Log ("found path from " + source + " to " + destination);
                    return ConstructPath(cameFrom, current, source);
                }


                openSet.Remove(current);
                fScore.Remove(current);
                --count;
                closedSet.Add(current);

                foreach (int neighbor in nodes[current].connections)
                {
                    if (closedSet.Contains(neighbor)) continue;
                    //Debug.Log("checking neighbor " + neighbor);
                    float tentativeGScore = gScore[current] + Vector3.Distance(nodes[current].pos, nodes[neighbor].pos);

                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Vector3.Distance(nodes[neighbor].pos, nodes[destination].pos);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                            ++count;
                        }
                    }
                }

            }

            return new List<Edge>();
        }


    }
}
