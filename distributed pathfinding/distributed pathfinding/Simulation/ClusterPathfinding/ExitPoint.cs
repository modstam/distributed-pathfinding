﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace distributed_pathfinding.Simulation.ClusterPathfinding
{
    class ExitPoint
    {
        public int x, y;
        public string id;
        public ExitPoint connection;


        public ExitPoint(int x, int y, ExitPoint connection)
        {
            this.x = x;
            this.y = y;
            this.id = x + "," + y;
            this.connection = connection;
        }

        public ExitPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.id = x + "," + y;
        }
    }
}
