using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;

namespace distributed_pathfinding.Simulation
{
    class SimulationMaster
    {

        NameValueCollection settings;
        Window mainWindow;
        Simulation simulation;
        Map map;


        public SimulationMaster(Window mainWindow)
        {
            getConfig();
            this.mainWindow = mainWindow;
            initiateSimulation();
           
        }

        private void getConfig()
        {
            settings = ConfigurationManager.AppSettings;
            if (settings != null)
            {
                foreach (var key in settings.AllKeys)
                {
                    Debug.WriteLine("Loading settings....");
                    Debug.WriteLine(key + "=" + settings[key]);
                    
                }
                Debug.WriteLine("Loading settings complete.");
            }
            else Debug.WriteLine("Warning: Config was not loaded---");

        }


        private void initiateSimulation()
        {
            if (settings["Map"] != null)
            {
                map = new Map();
                map.loadMap(settings["Map"]);
                simulation = new Simulation(map);
            }
            else
            {
                Debug.WriteLine("Could not read map");
            }

        }

        public void stop()
        {
            simulation.stop();
        }

        public void start()
        {
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


    }
}
