using System;
using System.Windows.Forms;

namespace LeakInterface
{
    internal static class Program
    {//SequenceUsed/NameSec

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());

        }
    }
}
