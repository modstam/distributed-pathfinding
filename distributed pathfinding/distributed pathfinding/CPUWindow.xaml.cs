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
        }
    }
}
