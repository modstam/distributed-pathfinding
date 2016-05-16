using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using distributed_pathfinding.Utility;

namespace distributed_pathfinding.Simulation
{
    class CPUManager
    {
        public CPUInfo getInfo()
        {
            CPUInfo info = new CPUInfo();

            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                info.ownerName = (string) item["Name"];
            }

            foreach (var item in new ManagementObjectSearcher("select * from Win32_Processor").Get())
            {   
                if(item != null)
                {
                    info.cpuFrequency = item["MaxClockSpeed"].ToString();
                    info.numLogicalProcessors = item["NumberOfLogicalProcessors"].ToString();
                    info.cpuModel = (string) item["Name"];
                }

            }


            Out.WriteLine(info.ownerName);
            Out.WriteLine(info.numLogicalProcessors);
            Out.WriteLine(info.cpuFrequency);
            Out.WriteLine(info.cpuModel);
            return info;
        }
    }
}
