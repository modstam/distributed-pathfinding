﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;
using distributed_pathfinding.Utility;
using distributed_pathfinding.Networking;

namespace distributed_pathfinding.Simulation
{
    class SimulationMaster
    {

        NameValueCollection settings;
        Window mainWindow;
        Simulation simulation;
        Map map;
        Network network;
        int numAgents=100;
        bool simplePath;

        public SimulationMaster(Window mainWindow, Network network, bool simplePath)
        {
            this.simplePath = simplePath;
            this.network = network;
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
                    Out.put("Loading settings....");
                    Out.put(key + "=" + settings[key]);
                    
                }
                Out.put("Loading settings complete.");
            }
            else Out.put("Warning: Config was not loaded---");

        }


        private void initiateSimulation()
        {
            if (settings["Map"] != null)
            {
                map = new Map();
                map.loadMap(settings["Map"]);
                simulation = new Simulation(map,simplePath);
                simulation.setNumAgents(numAgents);
            }
            else
            {
                Out.put("Could not read map");
            }

        }

        public void stop()
        {
           
            simulation.stop();
            network.stop();
        }

        public void start()
        {
            
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

        public int getNumAgents()
        {
            return simulation.getNumAgents();
        }

        public Map getMapCopy()
        {
            return simulation.getMapCopy();
        }

        public void setClusterSize(int clusterSize)
        {
            simulation.setClusterSize(clusterSize);
        }
    }
}
