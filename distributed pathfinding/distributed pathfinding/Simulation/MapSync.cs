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
        Map map = null;
        private object lockObject = new object();
        private bool flag = true;

        public void putProducedMap(Map map)
        {
            lock (lockObject)
            {
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
                this.map = map;
                flag = false;
                Monitor.Pulse(lockObject);
               
            }
           
        }

        public Map getProducedMap()
        {
            lock (lockObject)
            {
                while (flag)
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
                flag = true;
                Monitor.Pulse(lockObject);
                return map;
            }
        }
    }
}
