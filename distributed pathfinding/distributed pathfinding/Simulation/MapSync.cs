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
        private static Map map = new Map();
        private static object lockObject = new object();
        private static bool newMap = true;

        public static void putProducedMap(Map source)
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
                map = source;
                newMap = true;
                //Debug.WriteLine("Finished to put map...");
                Monitor.Pulse(lockObject);
               
            }
           
        }

        public static Map getProducedMap()
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
                return map;
            }
        }
    }
}
