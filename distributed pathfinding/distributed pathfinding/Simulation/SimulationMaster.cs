using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;
using distributed_pathfinding.Utility;

namespace distributed_pathfinding.Simulation
{
    class SimulationMaster
    {

        NameValueCollection settings;
        Window mainWindow;
        Simulation simulation;
        Map map;
        Networking networking;
        int numAgents=100;


        public SimulationMaster(Window mainWindow, Networking networking)
        {
            this.networking = networking;
            this.mainWindow = mainWindow;
            getConfig();
            initiateSimulation();
           
        }

        private void getConfig()
        {
            settings = ConfigurationManager.AppSettings;
            if (settings != null)
            {
                foreach (var key in settings.AllKeys)
                {
                    Out.WriteLine("Loading settings....");
                    Out.WriteLine(key + "=" + settings[key]);
                    
                }
                Out.WriteLine("Loading settings complete.");
            }
            else Out.WriteLine("Warning: Config was not loaded---");

        }


        private void initiateSimulation()
        {
            if (settings["Map"] != null)
            {
                map = new Map();
                map.loadMap(settings["Map"]);
                simulation = new Simulation(map);
                simulation.setNumAgents(numAgents);
            }
            else
            {
                Out.WriteLine("Could not read map");
            }

        }

        public void stop()
        {
            Out.WriteLine("Stopping simulation");
            simulation.stop();
        }

        public void start()
        {
            Out.WriteLine("Starting simulation");
            map.resetMap();
            simulation.start();
        }

        public int getMapWidth()
        {
            return map.getMapWidth();
        }

        public int getMapHeight()
        {
            return map.getMapHeight();
        }

        public string getMapURI()
        {
            if (settings["Map"] != null)
            {
                return settings["Map"];
            }
            return null;
        }

        public void setNumAgents(int numAgents)
        {
            this.numAgents = numAgents;
            simulation.setNumAgents(numAgents);
        }

    }
}
