using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public MainWindow()
        {
            setupMainWindow();
            InitializeComponent();
            Application.Current.MainWindow.Closing += new CancelEventHandler(mainWindowClosing);
            this.Loaded += new RoutedEventHandler(mainWindowLoaded);

            
        }

        public void setupMainWindow()
        {
            master = new SimulationMaster(this);
        }


        public void setupCPUWindow()
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
            Application.Current.Shutdown();
        }

        private void mainWindowLoaded(object sender, RoutedEventArgs e)
        {
            setupCPUWindow();
        }

    }
}
