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

namespace distributed_pathfinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Window CPUWindow;
        private Window outputWindow;
        private SimulationMaster master;
        private volatile bool shouldRun = false;
        private bool workerMode = false;
        private string ipAddress = "127.0.0.1";
        private Network network;
        private bool cpuWindow = true;
        private bool output = true;

        public MainWindow()
        {
            InitializeComponent();           
            Application.Current.MainWindow.Closing += new CancelEventHandler(mainWindowClosing);
            this.Loaded += new RoutedEventHandler(mainWindowLoaded);
            setupMainWindow();
            
        }


        private void runMap()
        {
            Out.put("Starting drawing agents..");

            List<Agent> agents = MapSync.getProducedAgents();
            Dictionary<int, UIElement> rectangles = null;


            while (shouldRun)
            {
                agents = MapSync.getProducedAgents();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    rectangles = updateAgents(agents, rectangles);
                }));
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                disposeAgents();
            }));

            Out.put("Agent drawing stopped...");
        }

        private void setupMainWindow()
        {
            this.network = new Network(workerMode, ipAddress);
            master = new SimulationMaster(this, this.network);
            List<Agent> agents = MapSync.getProducedAgents();
            setWindowSize(master.getMapHeight() + 200, master.getMapWidth() + 100);
            setUpCanvas(master.getMapHeight(), master.getMapWidth());
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
            CPUWindow.Close();
            stop();
            Application.Current.Shutdown();   
        }

        private void mainWindowLoaded(object sender, RoutedEventArgs e)
        {
            setupCPUWindow();
            setupOutputWindow();                        
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
                elements[agent.id] = addRect(agent.x, agent.y, 4, 4, Colors.Red, agent.id);

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

        private void submitIpButton_Click(object sender, RoutedEventArgs e)
        {
            IPAddress address;
            if (IPAddress.TryParse(ipTextBox.Text, out address))
            {
                Out.put("Valid ip adress entered: " + address.ToString());
            }
            else Out.put("Invalid ip adress entered: " + ipTextBox.Text);
        }

        private void modeCheckBox_Checked(object sender, RoutedEventArgs e)
        {

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
            if (!shouldRun) start();
            else stop();

        }
    }  
}
