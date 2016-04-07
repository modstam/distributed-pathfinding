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
using System.Windows.Shapes;
using distributed_pathfinding.Simulation;

namespace distributed_pathfinding
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CPUWindow : Window
    {

        private Window main_window;
        
        public CPUWindow(Window main)
        {
            this.main_window = main;
            WindowStartupLocation = WindowStartupLocation.Manual;
            InitializeComponent();

            populateList();

        }


        public void populateList()
        {
            CPUManager manager = new CPUManager();
            CPUInfo hostInfo = manager.getInfo();
            hostInfo.status = "HOST";

            List<CPUInfo> items = new List<CPUInfo>();
            items.Add(hostInfo);

            listView.ItemsSource = items;
        }
    }
}
