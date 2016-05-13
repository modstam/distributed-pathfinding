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

        public static List<Agent> getProducedAgents()
        {
            lock (lockObject)
            {
                while (!newMap)
                {
                    try
                    {
                        if(!Monitor.Wait(lockObject,10, true)) return null;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                }
                newMap = false;
                //Debug.WriteLine("Took map");
                List<Agent> agents = new List<Agent>(); 
                foreach(Agent agent in map.getAgents().Values)
                {
                    //Debug.WriteLine("Copy path: " + agent.getCopy().getPath().Count);                  
                }
                
                Monitor.Pulse(lockObject);
                return agents;
            }
        }
    }
}
