using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using distributed_pathfinding.Utility;

namespace distributed_pathfinding.Simulation
{
    class Map
    {

        Node[ , ] map;
        Bitmap img;
        Dictionary<int, Agent> agents;
        List<Node> nodes;
        
        public Map()
        {
            agents = new Dictionary<int, Agent>();
            nodes = new List<Node>();
        }

        public Map(Map prevMap)
        {
            this.map = prevMap.getMapCopy();
            this.img = prevMap.img;
            this.agents = null;
            this.nodes = null;
        }

        public void loadMap(string url)
        {

            Out.put("Current folder is " + System.AppDomain.CurrentDomain.BaseDirectory);
            try
            {
                img = new Bitmap(url);
                map = new Node[img.Width,img.Height];
                int id = 0;

                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        Color pixel = img.GetPixel(i, j);

                        if (pixel.ToArgb () != -1)
                        {
                            Node tmp = new Node(id, i, j, NodeType.Wall);
                            nodes.Add(tmp);                        
                            map[i, j] = tmp;
                                                     
                        }
                        else
                        {
                            Node tmp = new Node(id, i, j, NodeType.Empty);
                            nodes.Add(tmp);
                            map[i, j] = tmp;
                        }

                        ++id;
                    }      
                }
                Out.put("Map fully read, num wall pixels: " + id);
            }
            catch (Exception e)
            {
                Out.put(e.ToString());
            }
           
        }

        private Node[,] getMapCopy()
        {
            if(map != null)
            {
                Node[,] copy = new Node[getMatrixRowSize(), getMatrixColumnSize()];
                for(int x = 0; x < getMatrixRowSize(); ++x)
                {
                    for (int y = 0; y < getMatrixColumnSize(); ++y)
                    {
                        copy[x, y] = map[x, y].getCopy();
                    }
                }
                return copy;
            }
            return null;
        }

        public Map getDelemited(int topleft, int bottomright)
        {
            return null;
        }

        public int getMatrixRowSize()
        {
            return map.GetLength(0);
        }

        public int getMatrixColumnSize()
        {
            return map.GetLength(1);
        }

        public int getMapWidth()
        {
            return img.Width;
        }

        public int getMapHeight()
        {
            return img.Height;
        }

        public Node getNode(int x, int y)
        {
            return map[x, y];
        }

        public Node getNode(int id)
        {
            return nodes[id];
        }

        public Agent getAgent(int id)
        {
            return agents[id];
        }

        public Agent getAgent(int x, int y)
        {
            if (map[x, y].type == NodeType.Agent) return map[x, y].agent;
            return null;
        }

        public bool isOpen(int x, int y)
        {
            if (x < 0 || y < 0) return false;
            if (x >= getMatrixRowSize() || y >= getMatrixColumnSize()) return false;
            if (map[x, y].type != NodeType.Wall) return true;

            return false;
        }

        public void addAgent(int id, int x, int y)
        {
            if(map[x,y].type == NodeType.Empty)
            {
                map[x, y].type = NodeType.Agent;
                map[x, y].agent = new Agent(id, x, y);
                agents[id] = map[x, y].agent;
            }
        }

        public void moveAgent(int id, int newX, int newY)
        {
            
            map[agents[id].x, agents[id].y].type = NodeType.Empty;
            map[agents[id].x, agents[id].y].agent = null;

            map[newX, newY].type = NodeType.Agent;
            map[newX, newY].agent = agents[id];

            agents[id].move(newX, newY);
        }

        public void moveAgent(int oldX, int oldY, int newX, int newY)
        {
            Agent agent = map[oldX, oldY].agent;
           
            map[oldX, oldY].type = NodeType.Empty;
            map[oldX, oldY].agent = null;

            map[newX, newY].type = NodeType.Agent;
            map[newX, newY].agent = agent;

            map[oldX, oldY].agent.move(newX, newY);
        }

        public List<Node> getNodes()
        {
            return nodes;
        }

        public Dictionary<int,Agent> getAgents()
        {
            return agents;
        }
        
        public void resetMap()
        {
            agents = new Dictionary<int, Agent>();
            foreach(Node node in nodes)
            {
                node.reset();
            }
        }



    }
}
