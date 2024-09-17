using IniParser.Model;
using IniParser;
using LeakInterface.Global;
using System;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LeakInterface.Windows._001_Home
{


    public partial class WeetechConfig : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        string TempZcode, TempState, TempPass, TempError;
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

        public WeetechConfig()
        {
            InitializeComponent();

        }

        private void WeetechConfig_Load(object sender, EventArgs e)
        {
            ExtractInfo();
            log();
            label1.Select();
        }

        private void ExtractInfo()
        {
            
            string iniFilePath = GlobalL.WeetechConfiguration;

            // Crear una instancia de IniFile
            IniFile iniFile = new IniFile(iniFilePath);
            this.Invoke((MethodInvoker)delegate {
                Zcode.Text = iniFile.Read("Config", "ZCode");
                State.Text = iniFile.Read("Config", "State");
                Pass.Text = iniFile.Read("Config", "Pass");   
                Error.Text = iniFile.Read("Config", "Error");
            });   
        }


        private void log()
        {
            string content = File.ReadAllText(GlobalL.L__Weetech);

            // Display the content
            LogFiles.Text = (content);
            LogFiles.DeselectAll();

        }

    }
}
