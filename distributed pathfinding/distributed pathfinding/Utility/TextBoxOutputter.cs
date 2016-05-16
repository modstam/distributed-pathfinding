using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Controls;

namespace distributed_pathfinding.Utility
{
    public class TextBoxOutputter : TextWriter
    {
        TextBox textBox = null;

        public TextBoxOutputter(TextBox output)
        {
            textBox = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            textBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBox.AppendText(value.ToString());
                textBox.ScrollToEnd();
            }));
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    public class Out
    {
        public static void WriteLine(string s)
        {
            Debug.WriteLine(s);
            Console.WriteLine(s);
        }
    }
}
