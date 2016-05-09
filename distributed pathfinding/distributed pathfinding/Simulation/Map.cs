using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace distributed_pathfinding.Simulation
{
    class Map
    {

        Actor[ , ] map;
        Bitmap img;
        Dictionary<int, Actor> agents;


        public Map()
        {
            agents = new Dictionary<int, Actor>();
        }

        public void loadMap(string url)
        {

            Debug.WriteLine("Current folder is " + System.AppDomain.CurrentDomain.BaseDirectory);
            try
            {
                img = new Bitmap(url);
                map = new Actor[img.Width,img.Height];
                int id = 0;

                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        Color pixel = img.GetPixel(i, j);

                        if (pixel.ToArgb () != -1)
                        {
                            Actor tmp = new Actor(id, i, j, ActorType.Wall);
                            agents.Add(id, tmp);
                            map[i, j] = tmp;
                            ++id;                           
                        }
                        else
                        {
                            map[i, j] = null;
                        }                     
                    }      
                }
                Debug.WriteLine("Map fully read, num wall pixels: " + id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
           
        }

        public int[][] getInstance()
        {

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

        public Actor getAgent(int x, int y)
        {
            return map[x, y];
        }
        
        public Actor getAgent(int id)
        {
            return agents[id];
        }


    }
}
