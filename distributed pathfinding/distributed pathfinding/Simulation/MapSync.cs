using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security;
using System.Diagnostics;
namespace distributed_pathfinding.Simulation
{
    class MapSync
    {
        private static List<Node> nodes = new List<Node>();
        private static object lockObject = new object();
        private static bool newMap = true;

        public static void putProducedMap(List<Node> source)
        {
            lock (lockObject)
            {
                /*
                while (!flag)
                {
                    try
                    {
                       
                        Monitor.Wait(lockObject);
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                }
                */
                nodes = source;
                newMap = true;
                //Debug.WriteLine("Finished to put map...");
                Monitor.Pulse(lockObject);
               
            }
           
        }

        public static List<Node> getProducedMap()
        {
            lock (lockObject)
            {
                while (!newMap)
                {
                    try
                    {
                        Monitor.Wait(lockObject);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                }
                newMap = false;
                //Debug.WriteLine("Took map");
                Monitor.Pulse(lockObject);
                return nodes;
            }
        }
    }
}
