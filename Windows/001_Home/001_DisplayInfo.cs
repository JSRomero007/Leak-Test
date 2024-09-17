using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeakInterface.Windows._001_Home
{
    public partial class DisplayInfo : Form
    {
        public DisplayInfo()
        {
            InitializeComponent();
        }
        public class IniFile
        {
            private string _path;

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            private static extern int GetPrivateProfileString(
                string section,
                string key,
                string defaultValue,
                StringBuilder value,
                int size,
                string filePath);

            public IniFile(string path)
            {
                _path = path;
            }

            public string Read(string section, string key)
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(section, key, "", temp, 255, _path);
                return temp.ToString();
            }
        }
        private void DisplayInfo_Load(object sender, EventArgs e)
        {

        }


    }
}
