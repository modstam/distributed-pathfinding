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

        int[,] map;

        public Map()
        {


        }

        public void loadMap(string url)
        {

            Debug.WriteLine("Current folder is " + System.AppDomain.CurrentDomain.BaseDirectory);
            try
            {
                Bitmap img = new Bitmap(url);
                map = new int[img.Width,img.Height];

                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        Color pixel = img.GetPixel(i, j);

                        if (pixel.ToArgb () != -1)
                        {
                            map[i, j] = 1;
                        }
                        else
                        {
                            map[i, j] = 0;
                        }
                    }
                }

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

        
    }
}
