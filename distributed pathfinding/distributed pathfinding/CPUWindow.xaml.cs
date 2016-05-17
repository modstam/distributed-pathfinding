using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;
using distributed_pathfinding.Simulation;
using distributed_pathfinding.Networking;
using distributed_pathfinding.Utility;
using System.Collections.ObjectModel;

namespace distributed_pathfinding
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CPUWindow : Window
    {

        private Window main_window;


        private volatile bool shouldRun = true;

        private ObservableCollection<CPUInfo> list;
        
        public CPUWindow(Window main)
        {
            this.main_window = main;
            WindowStartupLocation = WindowStartupLocation.Manual;
            InitializeComponent();

            list = new ObservableCollection<CPUInfo>();
            getHostInfo();

            listView.ItemsSource = list;
            Thread dataCheck = new Thread(collectData);
            dataCheck.Start();


        }

        private void collectData()
        {
            while (shouldRun)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (SystemDelegator.hasNew())
                    {
                        var newInfo = SystemDelegator.getNewCPUs();
                        foreach(CPUInfo info in newInfo)
                        {
                            list.Add(info);
                        }
                    }
                }));
                Thread.Sleep(1000);
            }
        }

        private void getHostInfo()
        {
            CPUManager manager = new CPUManager();
            var hostInfo = CPUManager.getInfo();
            hostInfo.status = "HOST";
            list.Add(hostInfo);
        }

        public void stop()
        {
            shouldRun = false;
        }



    }
}
