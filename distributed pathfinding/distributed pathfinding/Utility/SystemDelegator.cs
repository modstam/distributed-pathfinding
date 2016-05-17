using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using distributed_pathfinding.Networking;

namespace distributed_pathfinding.Utility
{
    class SystemDelegator
    {
        private static List<CPUInfo> connectedCPUs= new List<CPUInfo>();
        private static int numCPU;
        private static List<CPUInfo> newCPUs = new List<CPUInfo>();
        private static bool existsNew = false;

        public static int addCPU(CPUInfo cpu)
        {

            if (!existsNew)
            {
                newCPUs = new List<CPUInfo>();
            }
            cpu.id = numCPU;
            numCPU++;
            connectedCPUs.Add(cpu);
            newCPUs.Add(cpu);
            existsNew = true;
            return numCPU;
        }

        public static void removeCPU(string id)
        {

        }

        public static CPUInfo getCPU(string ID)
        {
            return null;
        }

        public static List<CPUInfo> getAllCPUs()
        {   
            
            return connectedCPUs;
        }

        public static List<CPUInfo> getNewCPUs()
        {
            if(!existsNew)
            {
                newCPUs = new List<CPUInfo>();
                return newCPUs;
            }
            else
            {
                existsNew = false;
                return newCPUs;
            }

        }

        public static bool hasNew()
        {
            return existsNew;
        }

    }
}
