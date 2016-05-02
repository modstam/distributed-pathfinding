using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation
{

    class CPUInfo
    {
        public string status { get; set; }
        public string cpuModel { get; set; }
        public string cpuFrequency { get; set; }
        public string ownerName { get; set; }
        public string numLogicalProcessors { get; set; }
        public string squares { get; set; }
        public string rate { get; set; }
        public string ip { get; set; }

        public CPUInfo()
        {

        }

    }
}
