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
            setupMainWindow();
            InitializeComponent();
            Application.Current.MainWindow.Closing += new CancelEventHandler(mainWindowClosing);
            this.Loaded += new RoutedEventHandler(mainWindowLoaded);


        }

        private void setupMainWindow()
        {
            master = new SimulationMaster(this);  
            start();
                               
        }

        private void setWindowSize(int height, int width)
        {

            Application.Current.MainWindow.Height = height;
            Application.Current.MainWindow.Width = width;

            Debug.WriteLine("Resized main window..");
        }

        private void setCanvasSize(int height, int width)
        {

            mapCanvas.Height = height;
            mapCanvas.Width = width;

            Debug.WriteLine("Resized canvas window..");
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
           
            CPUWindow.Close();
            stop();
            Application.Current.Shutdown();   
        }

        private void mainWindowLoaded(object sender, RoutedEventArgs e)
        {
            setupCPUWindow();
            Map map = MapSync.getProducedMap();
            setWindowSize(map.getMapHeight() + 100, map.getMapWidth() + 100);
            setCanvasSize(map.getMapHeight(), map.getMapWidth());

            for(int x = 0; x < map.getMatrixRowSize(); x++)
            {
                for (int y = 0; y < map.getMatrixRowSize(); y++)
                {
                    if (map.getAgent(x, y) == null) continue;
                    if (map.getAgent(x, y).type == ActorType.Wall) addRect(x, y, 1, 1);
                }
            }
            
        }

        private void runMap()
        {
            Debug.WriteLine("Starting mapUpdateThread..");
            while (shouldRun)
            {
                Map map = MapSync.getProducedMap();
               // Debug.WriteLine("Acquired map");
            }
            Debug.WriteLine("Map updating stopped...");
        }
        
        private void stop()
        {
            shouldRun = false;
            master.stop();           
        }

        private void start()
        {
            shouldRun = true;
            master.start();
            Thread mapUpdateThread = new Thread(runMap);
            mapUpdateThread.Start();
        }


        private void addRect(double x, double y, int h, int w)
        {
            Rectangle rec = new Rectangle();
            Canvas.SetTop(rec, y);
            Canvas.SetLeft(rec, x);
            rec.Width = w;
            rec.Height = h;
            rec.Fill = new SolidColorBrush(Colors.Red);
            mapCanvas.Children.Add(rec);
        }


    }  
}
