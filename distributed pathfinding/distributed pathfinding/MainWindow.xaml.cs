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

namespace distributed_pathfinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Window CPUWindow;
        private SimulationMaster master;
        private volatile bool shouldRun = false;

        public MainWindow()
        {
            InitializeComponent();           
            Application.Current.MainWindow.Closing += new CancelEventHandler(mainWindowClosing);
            this.Loaded += new RoutedEventHandler(mainWindowLoaded);
            master = new SimulationMaster(this);
            setupMainWindow();
            start();
        }

        private void setupMainWindow()
        {    
            List<Agent> agents = MapSync.getProducedAgents();
            setWindowSize(master.getMapHeight() + 100, master.getMapWidth() + 100);
            setUpCanvas(master.getMapHeight(), master.getMapWidth());
        }

        private void setWindowSize(int height, int width)
        {

            Application.Current.MainWindow.Height = height;
            Application.Current.MainWindow.Width = width;

            Debug.WriteLine("Resized main window..");
        }

        private void setUpCanvas(int height, int width)
        {
            mapCanvas.Height = height;
            mapCanvas.Width = width;

            Debug.WriteLine("Resized canvas window..");

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

        private void mainWindowClosing(object sender, CancelEventArgs e)
        {
            //lets close our CPU-window when we close the main window
            Debug.WriteLine("Closing main window");
            CPUWindow.Close();
            stop();
            Application.Current.Shutdown();   
        }

        private void mainWindowLoaded(object sender, RoutedEventArgs e)
        {
            setupCPUWindow();
            
            /*
            foreach(Node node in map)
            {
                if (node.type == NodeType.Wall) addRect(node.x, node.y, 1, 1);
            } 
            */              
        }

        
        private void runMap()
        {
            Debug.WriteLine("Starting drawing agents..");

            List<Agent> agents = MapSync.getProducedAgents();
            Dictionary<int, UIElement> rectangles = null;


            while (shouldRun)
            {
                agents = MapSync.getProducedAgents();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    rectangles = updateAgents(agents,rectangles);
                }));
            }
            Debug.WriteLine("Agent drawing stopped...");
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
                elements[agent.id] = addRect(agent.x, agent.y, 4, 4, Colors.Red, agent.id);

                //Debug.WriteLine("Agent: " + agent.id + " detected ");
                /**
                if(agent.getPath() != null)
                {
                    foreach (Node node in agent.getPath())
                    {
                        addRect(node.x, node.y, 1, 1, Colors.Red, agent.id);
                    }
                }
                **/

            }
            return elements;
        }
        
        private void stop()
        {
            shouldRun = false;
            master.stop();           
        }

        private void start()
        {
            shouldRun = true;

            Thread mapUpdateThread = new Thread(runMap);
            mapUpdateThread.Start();

            Thread simulationThread = new Thread(master.start);
            simulationThread.Start();

            
        }


        private Rectangle addRect(double x, double y, int h, int w, Color color, int id)
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


    }  
}
