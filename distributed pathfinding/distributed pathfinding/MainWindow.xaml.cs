using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using distributed_pathfinding.Simulation;
using System.Net;
using distributed_pathfinding.Utility;
using distributed_pathfinding.Networking;
using distributed_pathfinding.Simulation.ClusterPathfinding;

namespace distributed_pathfinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CPUWindow CPUWindow;
        private OutputWindow outputWindow;
        private SimulationMaster master;
        private volatile bool simulationRun = false;
        private bool workerMode = false;
        private string ipAddress = "127.0.0.1";
        private Network network;
        private bool cpuWindow = true;
        private bool output = true;
        private bool networkRun = false;
        private bool simplePath = true;
        private int clusterSize = 100;

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.Closing += new CancelEventHandler(mainWindowClosing);
            this.Loaded += new RoutedEventHandler(mainWindowLoaded);
            setupMainWindow();

        }

        private void setupMainWindow()
        {
            this.network = new Network(workerMode, ipAddress);
            master = new SimulationMaster(this, this.network, this.simplePath);
            master.setClusterSize(clusterSize);
            List<Agent> agents = MapSync.getProducedAgents();
            setWindowSize(master.getMapHeight() + 200, master.getMapWidth() + 100);
            setUpCanvas(master.getMapHeight(), master.getMapWidth());
            numAgents.Text = "" + master.getNumAgents();
            ipTextBox.Text = "" + ipAddress;
        }

        private void setWindowSize(int height, int width)
        {

            Application.Current.MainWindow.Height = height;
            Application.Current.MainWindow.Width = width;

            Out.put("Resized main window..");
        }


        private void setUpCanvas(int height, int width)
        {
            mapCanvas.Height = height;
            mapCanvas.Width = width;

            Out.put("Resized canvas window..");

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(master.getMapURI(), UriKind.Relative));
            mapCanvas.Background = ib;
        }



        private void setupCPUWindow()
        {
            CPUWindow = new CPUWindow(this);
            CPUWindow.Left = Application.Current.MainWindow.Left + Application.Current.MainWindow.ActualWidth;
            CPUWindow.Top = Application.Current.MainWindow.Top;
            CPUWindow.Show();
        }

        private void setupOutputWindow()
        {
            outputWindow = new OutputWindow();
            outputWindow.Left = Application.Current.MainWindow.Left + Application.Current.MainWindow.ActualWidth;
            outputWindow.Top = Application.Current.MainWindow.Top;
            outputWindow.Show();
        }

        private void mainWindowClosing(object sender, CancelEventArgs e)
        {
            //lets close our CPU-window when we close the main window
            Out.put("Closing main window");
            CPUWindow.stop();
            CPUWindow.Close();
            outputWindow.Close();
            stop();
            Application.Current.Shutdown();
        }

        private void mainWindowLoaded(object sender, RoutedEventArgs e)
        {
            setupCPUWindow();
            setupOutputWindow();
        }


        private void runMap()
        {
            Out.put("Starting drawing agents..");

            List<Agent> agents = MapSync.getProducedAgents();
            Dictionary<int, UIElement> rectangles = null;
            bool init = true;
            var copyMap = master.getMapCopy();
            var clusterGen = new ClusterGenerator(clusterSize);
            var clusters = clusterGen.generateClusters(copyMap);
            //drawPaths(clusters);

            while (simulationRun)
            {
                agents = MapSync.getProducedAgents();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (init)
                    {
                        drawClusters(clusters);
                        //drawPaths(clusters);
                        init = false;
                    }
                    rectangles = updateAgents(agents, rectangles);
                }));
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                disposeAgents();
            }));

            Out.put("Agent drawing stopped...");
        }

        private List<UIElement> drawClusters(Cluster[,] clusters)
        {
            var rectangles = new List<UIElement>();
            int id = 10000000;
            foreach(Cluster cluster in clusters)
            {
                rectangles.Add(addRect(cluster.left, cluster.top, cluster.height, cluster.width, Colors.Blue,1, id));
                //Out.put(cluster.left + "," + cluster.top + "," + cluster.height + "," + cluster.width);
                ++id;
                drawExits(cluster);
            }

            return rectangles;
        }

        private void drawExits(Cluster cluster)
        {
            int size = 5;
            foreach(ExitPoint exit in cluster.exits)
            {
                if (exit.x + size >= cluster.left + cluster.width && exit.y + size >= cluster.top)
                {
                    int moveX = cluster.left - exit.x + size;
                    int moveY = cluster.top - exit.y + size;

                    moveX = 0;
                    moveY = 0;

                    addFilledRect(exit.x + moveX, exit.y + moveY, size, size, Colors.Green, 0);
                }
                else if(exit.x + size >= cluster.left + cluster.width)
                {
                    int moveX = cluster.left - exit.x + size;

                    moveX = 0;

                    addFilledRect(exit.x+moveX, exit.y, size, size, Colors.Green, 0);
                }
                else
                {
                    addFilledRect(exit.x, exit.y, size, size, Colors.Green, 0);
                }                                        
            }         
        }

        private void drawPaths(Cluster[,] clusters)
        {
            foreach(Cluster cluster in clusters)
            {

                foreach(ExitPoint exit1 in cluster.exits)
                {
                    foreach (ExitPoint exit2 in cluster.exits)
                    {
                        if (exit1 == exit2) continue;
                        var path = cluster.getPath(exit1, exit2);
                        foreach (var node in path)
                        {
                            addFilledRect(node.x, node.y, 2, 2, Colors.DarkMagenta, 0);
                        }
                    }
                }
            }
        }


        private void disposeAgents()
        {
            mapCanvas.Children.Clear();
        }

        private Dictionary<int, UIElement> updateAgents(List<Agent> agents, Dictionary<int, UIElement> rects)
        {
            if (agents != null && rects != null)
            { 
                foreach(Agent agent in agents)
                {
                    Canvas.SetLeft(rects[agent.id], agent.x);
                    Canvas.SetTop(rects[agent.id], agent.y);
                }
            }
            else if (agents != null && rects == null )
            {
                rects = drawAgents(agents);
            }

            return rects;           
       
        }

        private Dictionary<int, UIElement> drawAgents(List<Agent> agents)
        {
            Dictionary<int, UIElement> elements = new Dictionary<int, UIElement>();
            if (agents == null) return null;
            foreach (Agent agent in agents)
            {
                elements[agent.id] = addFilledRect(agent.x, agent.y, 2, 2, Colors.Red, agent.id);

            }
            return elements;
        }
        
        private void stop()
        {
            simulationRun = false;
            master.stop();           
        }

        private void start()
        {
            simulationRun = true;

            Thread mapUpdateThread = new Thread(runMap);
            mapUpdateThread.Start();

            Thread simulationThread = new Thread(master.start);
            simulationThread.Start();     
        }


        private Rectangle addFilledRect(double x, double y, int h, int w, Color color, int id)
        {
            Rectangle rec = new Rectangle();
            rec.Uid = "" + id;
            Canvas.SetTop(rec, y);
            Canvas.SetLeft(rec, x);
            rec.Width = w;
            rec.Height = h;
            rec.Fill = new SolidColorBrush(color);
            mapCanvas.Children.Add(rec);  

            return rec;
        }

        private Rectangle addRect(double x, double y, int h, int w, Color color, int strokeSize, int id)
        {
            Rectangle rec = new Rectangle();
            rec.Uid = "" + id;
            Canvas.SetTop(rec, y);
            Canvas.SetLeft(rec, x);
            rec.Width = w;
            rec.Height = h;
            rec.Stroke = new SolidColorBrush(color);
            rec.StrokeThickness = strokeSize;
            mapCanvas.Children.Add(rec);

            return rec;
        }

        private void networkButton_Click(object sender, RoutedEventArgs e)
        {                   
                
            if (!networkRun)
            {
                stop();
                network.stop();
                networkRun = true;
                networkButton.Content = "Stop";
                if (workerMode)
                {
                IPAddress address;
                if (IPAddress.TryParse(ipTextBox.Text, out address))
                    {
                    Out.put("Valid ip adress entered: " + address.ToString());
                    network = new Network(workerMode, address.ToString());
                    }
                    else Out.put("Invalid ip adress entered: " + ipTextBox.Text);
                    }
                else
                {
                    network = new Network(workerMode);
                }
                network.start();                 
            }
            else
            {
                stop();
                network.stop();
                networkRun = false;
                networkButton.Content = "Listen";
             }
        }

        private void modeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (modeCheckBox.IsChecked == true)
            {
                workerMode = true;
            }
            else
            {
                workerMode = false;
            }
        }

        private void numAgents_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            int result;
            if (int.TryParse(textBox.Text, out result))
            {
                master.setNumAgents(result);
                Out.put("Changed num agents to: " + result);
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (!simulationRun)
            {
                startButton.Content = "Stop";
                start();
            }
            else
            {
                startButton.Content = "Start";
                stop();
            }

        }
    }  
}
