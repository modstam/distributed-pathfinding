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
using System.Diagnostics;
using System.ComponentModel;
using distributed_pathfinding.Utility;

namespace distributed_pathfinding
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {

        TextBoxOutputter outputter;

        public OutputWindow()
        {
            InitializeComponent();
            Closing += new CancelEventHandler(closeWindow);

            outputter = new TextBoxOutputter(textBox);
            Console.SetOut(outputter);
            Console.WriteLine("Waiting for output...");
        }

        public void closeWindow(object sender, CancelEventArgs e)
        {
            Console.OpenStandardOutput();
            Out.put("Closing outputwindow");
        }

    }
}
