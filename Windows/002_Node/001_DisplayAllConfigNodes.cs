using LeakInterface.Global;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;



namespace LeakInterface.Windows._002_Node
{
    public partial class DisplayAllNodes : Form
    {
        string NodePort = GlobalL.PortConfig;
        int NodeuseToint;
        string PortCOM;
        public class IniFile
        {
            private string _path;

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

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
            public void Write(string section, string key, string value)
            {
                WritePrivateProfileString(section, key, value, _path);
            }

        }
        public DisplayAllNodes()
        {
            InitializeComponent();
            LoadInfo();
        }
        private void LoadInfo()
        {
            IniFile ini = new IniFile(NodePort);
            GlobalV.PortCom = (ini.Read("ConfigNodes", "PortCom"));
            GlobalV.NodesUse = (ini.Read("ConfigNodes", "NodesUse"));
            NodeuseToint = int.Parse(ini.Read("ConfigNodes", "NodesUse"));
            PortCOM=GlobalV.PortCom;
        }

        public void DisplayActualConfigList()
        {
            HistoryData.Clear();
            HistoryData.Text = " Port configurate:" + PortCOM + Environment.NewLine;
            int Count = 1;
            while (Count <= NodeuseToint)
            {

                HistoryData.AppendText("     ●Node-" + Count + Environment.NewLine);
                HistoryData.AppendText("         └Sesor " + Count + "-A" + Environment.NewLine);
                HistoryData.AppendText("         └Sesor " + Count + "-B" + Environment.NewLine);
                Count++;

            }
        }
        private void DisplayAllNodes_Load(object sender, EventArgs e)
        {

            DisplayActualConfigList();
        }
       
    }
}
