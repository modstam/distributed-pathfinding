using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using distributed_pathfinding.Utility;

namespace distributed_pathfinding.Simulation.ClusterPathfinding
{
    class ClusterGenerator
    {
        int size;

        public ClusterGenerator(int size)
        {
            this.size = size;
        }

        public Cluster[,] generateClusters(Map map)
        {
            var clusters = makeClusters(map);
            generateExits(map, clusters);

            return clusters;
        }

        private Cluster[,] makeClusters(Map map)
        {

            int x = calcClusterXSize(map);
            int y = calcClusterYSize(map);

            var clusters = new Cluster[map.getMatrixRowSize() / x, map.getMatrixColumnSize() / y];

            Out.put("Closest cluster size: " + x + "x" + y);
            int c = 0, k = 0;

            for (int i = 0; i < map.getMatrixRowSize(); i += x, ++c)
            {
                for (int j = 0; j < map.getMatrixColumnSize(); j += y, ++k)
                {
                    var cluster = new Cluster(j, i, x, y, c, k);
                    clusters[c, k] = cluster;
                }
                k = 0;
            }

            return clusters;
        }

        private void generateExits( Map map, Cluster[,] clusters)
        {
            for(int x = 0; x < clusters.GetLength(0); ++x)
            {
                for (int y = 0; y < clusters.GetLength(1); ++y)
                {   
                    if(y >= 1)
                    {
                        var cluster1 = clusters[x, y];
                        var cluster2 = clusters[x, y - 1];
                        makeExitHorizontal(cluster1, cluster2, map, 1);
                        makeMiddleExitHorizontal(cluster1, cluster2, map);
                        makeExitHorizontal(cluster1, cluster2, map, -1);
                    }
                    if(x >= 1)
                    {
                        var cluster1 = clusters[x, y];
                        var cluster2 = clusters[x-1, y];
                        makeExitVertical(cluster1, cluster2, map, 1);
                        makeMiddleExitVertical(cluster1, cluster2, map);
                        makeExitVertical(cluster1, cluster2, map, -1);
                    }

                }
            }

        }

        private void makeExitVertical(Cluster cluster1, Cluster cluster2, Map map, int direction)
        {
            int h = cluster1.height;
            int x = cluster1.left;
            int y = (direction >= 0) ? cluster1.top : cluster1.top + h;
            //go right until we find the first spot
            while (Math.Abs(y - cluster1.top + h / 2) != 0)
            {
                if (map.isOpen(x, y) && map.isOpen(x -1, y))
                {
                    cluster1.addExit(x, y);
                    cluster2.addExit(x-1, y);
                    return;
                }
                y += direction;
            }
        }

        private void makeExitHorizontal(Cluster cluster1, Cluster cluster2, Map map, int direction)
        {
            int w = cluster1.width;
            int y = cluster1.top;
            int x = (direction >= 0) ? cluster1.left : cluster1.left + w; 
            //go right until we find the first spot
            while(Math.Abs(x - cluster1.left + w/2) != 0)
            {
                if (map.isOpen(x, y) && map.isOpen(x, y - 1))
                {
                    cluster1.addExit(x, y);
                    cluster2.addExit(x, y - 1);
                    return;
                }
                x += direction;
            }
        }

        private void makeMiddleExitVertical(Cluster cluster1, Cluster cluster2, Map map)
        {
            int h = cluster1.height;
            int x = cluster1.left;
            int firstOpenTop = -1;
            int firstOpenBottom = -1;
            //go top from middle first;
            for (int y = cluster1.top + h / 2; y < cluster1.top + h; ++y)
            {
                //if there is an exit available on both clusters
                if (map.isOpen(x, y) && map.isOpen(x-1, y))
                {
                    firstOpenBottom = y;
                    break;
                }
            }
            //go bottom from middle next;
            for (int y = cluster1.top + (h / 2) - 1; 0 < y; --y)
            {
                //if there is an exit available on both clusters
                if (map.isOpen(x, y) && map.isOpen(x-1, y))
                {
                    firstOpenTop = y;
                    break;
                }
            }
            int selectedY = 0;
            if (firstOpenTop + firstOpenBottom == -2) return;
            else if (firstOpenTop == -1) selectedY = firstOpenBottom;
            else if (firstOpenBottom == -1) selectedY = firstOpenTop;
            else selectedY = (Math.Abs(firstOpenBottom) < Math.Abs(firstOpenTop)) ? firstOpenBottom : firstOpenTop;   //compare which side is closest to middle

            cluster1.addExit(x, selectedY);
            cluster2.addExit(x-1,selectedY);

        }


        private void makeMiddleExitHorizontal(Cluster cluster1, Cluster cluster2, Map map)
        {
            int w = cluster1.width;
            int y = cluster1.top;
            int firstOpenLeft = -1;
            int firstOpenRight = -1;
            //go right from middle first;
            for(int x = cluster1.left + w/2 ; x < cluster1.left + w; ++x)
            {
                //if there is an exit available on both clusters
                if(map.isOpen(x,y) && map.isOpen(x,y-1))
                {
                    firstOpenRight = x;
                    break;
                }
            }
            //go left from middle next;
            for (int x = cluster1.left + (w / 2) - 1; 0 < x; --x)
            {
                //if there is an exit available on both clusters
                if (map.isOpen(x, y) && map.isOpen(x, y - 1))
                {
                    firstOpenLeft = x;
                    break;
                }
            }
            int selectedX = 0;
            if (firstOpenLeft + firstOpenRight == -2) return;
            else if (firstOpenLeft == -1) selectedX = firstOpenRight;
            else if(firstOpenRight == -1) selectedX = firstOpenLeft;
            else selectedX = (Math.Abs(firstOpenRight) < Math.Abs(firstOpenLeft)) ? firstOpenRight : firstOpenLeft;   //compare which side is closest to middle

            cluster1.addExit(selectedX, y);
            cluster2.addExit(selectedX, y - 1);

        }

        private int calcClusterXSize(Map map)
        {
            int i = size;
            while (map.getMatrixRowSize() % i != 0 && i > 0) 
            {
                --i;
            }
            if (i == 0) return 0;
            return i;
        }

        private int calcClusterYSize(Map map)
        {
            int i = size;
            while (map.getMatrixColumnSize() % i != 0 && i > 0)
            {
                --i;
            }
            if (i == 0) return 0;
            return i;
        }
    }
}
