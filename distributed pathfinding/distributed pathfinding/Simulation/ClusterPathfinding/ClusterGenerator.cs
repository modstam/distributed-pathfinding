﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using distributed_pathfinding.Utility;

namespace distributed_pathfinding.Simulation.ClusterPathfinding
{
    class ClusterGenerator
    {
        int size;
        Map map;

        public ClusterGenerator(int size)
        {
            this.size = size;
        }

        public ClusterMap generateClusters(Map map)
        {
            this.map = map;
            var clusters = makeClusters(map);
            generateExits(clusters);
            makePathsBetweenExits(clusters);

            return clusters;
        }

        private ClusterMap makeClusters(Map map)
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
            var clusterMap = new ClusterMap(clusters, map);
            return clusterMap;
        }

        private void generateExits(ClusterMap clusterMap)
        {
            var clusters = clusterMap.getClusterMap();
            for(int x = 0; x < clusters.GetLength(0); ++x)
            {
                for (int y = 0; y < clusters.GetLength(1); ++y)
                {   
                    if(y >= 1)
                    {
                        var cluster1 = clusters[x, y];
                        var cluster2 = clusters[x, y - 1];
                        makeExitHorizontal(cluster1, cluster2, clusterMap, 1);
                        makeMiddleExitHorizontal(cluster1, cluster2, clusterMap);
                        makeExitHorizontal(cluster1, cluster2, clusterMap, -1);
                    }
                    if(x >= 1)
                    {
                        var cluster1 = clusters[x, y];
                        var cluster2 = clusters[x-1, y];
                        makeExitVertical(cluster1, cluster2, clusterMap, 1);
                        makeMiddleExitVertical(cluster1, cluster2, clusterMap);
                        makeExitVertical(cluster1, cluster2, clusterMap, -1);
                    }

                }
            }

        }

        private void makeExitVertical(Cluster cluster1, Cluster cluster2, ClusterMap clusters, int direction)
        {
            var map = clusters.getMap();
            int h = cluster1.height;
            int x = cluster1.left;
            int y = (direction >= 0) ? cluster1.top : cluster1.top + h-1;
            //go right until we find the first spot
            while (Math.Abs(y - cluster1.top + h / 2) != 0)
            {
                if (map.isOpen(x, y) && map.isOpen(x -1, y))
                {
                    var exit1 = new ExitPoint(x, y);
                    exit1.connection = new ExitPoint(x - 1, y, exit1);
                    cluster1.addExit(exit1);
                    cluster2.addExit(exit1.connection);
                    return;
                }
                y += direction;
            }
        }

        private void makeExitHorizontal(Cluster cluster1, Cluster cluster2, ClusterMap clusters, int direction)
        {
            var map = clusters.getMap();
            int w = cluster1.width;
            int y = cluster1.top;
            int x = (direction >= 0) ? cluster1.left : cluster1.left + w-1; 
            //go right until we find the first spot
            while(Math.Abs(x - cluster1.left + w/2) != 0)
            {
                if (map.isOpen(x, y) && map.isOpen(x, y - 1))
                {

                    var exit1 = new ExitPoint(x, y);
                    exit1.connection = new ExitPoint(x, y-1, exit1);
                    cluster1.addExit(exit1);
                    cluster2.addExit(exit1.connection);
                    return;
                }
                x += direction;
            }
        }

        private void makeMiddleExitVertical(Cluster cluster1, Cluster cluster2, ClusterMap clusters)
        {
            var map = clusters.getMap();
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

            var exit1 = new ExitPoint(x, selectedY);
            exit1.connection = new ExitPoint(x - 1, selectedY, exit1);
            cluster1.addExit(exit1);
            cluster2.addExit(exit1.connection);

        }


        private void makeMiddleExitHorizontal(Cluster cluster1, Cluster cluster2, ClusterMap clusters)
        {
            var map = clusters.getMap();
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

            var exit1 = new ExitPoint(selectedX, y);
            exit1.connection = new ExitPoint(selectedX, y -1, exit1);
            cluster1.addExit(exit1);
            cluster2.addExit(exit1.connection);

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

        private void makePathsBetweenExits( ClusterMap clusterMap)
        {
            var clusters = clusterMap.getClusterMap();
            var threads = new List<Thread>();
            Stopwatch sw = new Stopwatch();

            sw.Start();
            foreach(Cluster cluster in clusters)
            {              
                Thread thread = new Thread(connectExitStarter);
                thread.Start(cluster);
            }

            foreach (var t in threads)
            {
                t.Join();
            }
            bool lives = false;
            foreach(var t in threads)
            {
                if (t.IsAlive) lives = true;
            }
            

            Out.put("active threads connecting exits: " + lives);
            Out.put("connecting exits took " + sw.ElapsedMilliseconds + "ms");
            sw.Stop();
        }

        private void connectExitStarter(object cluster)
        {
            Cluster clus = (Cluster)cluster;
            clus.connectExits(map);
        }      
        
    }
}
