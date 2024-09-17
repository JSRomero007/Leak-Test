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
    public partial class _002_LeakPort : Form
    {
        //--- Variables, Regex & path use ---\\

        Regex COM = new Regex(@"^COM([0-9]{1,3})$");
        Regex NodeS = new Regex(@"^[0-9]{1,2}$");
        string NodePort = GlobalL.PortConfig;
        string PortCOM, NodesUSE;
        int NodeuseToint;
        string TmpPort, TmpNode;
        string Hour = DateTime.Now.ToString("hh:mm tt");
        string Date = DateTime.Now.ToString("D");
        private static string path = GlobalL.L__Config;
        //-----------------------------------\\

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
        public _002_LeakPort()
        {
            InitializeComponent();
            ClearNewConfig();
            StartConfig();
        }
        private void StartConfig()
        {
            NodesU.Enabled = false;
            Save.Enabled = false;
            TestButton.Enabled = false;
            OkErrorLabel.Text = string.Empty;
        }
        public void DefaultPortValues()
        {
            SelectedPort.DropDownStyle = ComboBoxStyle.DropDownList;
            NodesU.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void DisplayNewConfig()
        {
            //---- Show items ----\\
            NewCOM.Visible = true;
            Box2.Visible = true;
            vs.Visible = true;
            NewNodeU.Visible = true;
            NewPortUs.Visible = true;

        }
        private void ClearNewConfig()
        {
            //---- Hide items ----\\
            NewCOM.Visible = false;
            Box2.Visible = false;
            vs.Visible = false;
            NewNodeU.Visible = false;
            NewPortUs.Visible = false;

            //---- Clear items ----\\
            NewPortUSE.Text = string.Empty;
            NewNodeUSE.Text = string.Empty;
            ActualPort.Text = string.Empty;
            ActualNode.Text = string.Empty;

        }
        private void AddItemsInNodes()
        {
            NodesU.Items.Clear();
            for (int i = 0; i <= 30; i++)
            {
                NodesU.Items.Add(i);
            }
        }
        private void DisplayNodePORTconfig()
        {
            //--- Save string in GlobalV ---\\
            IniFile ini = new IniFile(NodePort);
            GlobalV.PortCom = (ini.Read("ConfigNodes", "PortCom"));
            GlobalV.NodesUse = (ini.Read("ConfigNodes", "NodesUse"));
            NodeuseToint = int.Parse(ini.Read("ConfigNodes", "NodesUse"));
            PortCOM = GlobalV.PortCom; NodesUSE = GlobalV.NodesUse;
            //--- Actual Config Display ---\\
            ActualPort.Text = PortCOM;
            ActualNode.Text = NodesUSE;
            DisplayActualConfigList();
        }

        private void _002_LeakPort_Load(object sender, EventArgs e)
        {
            DisplayNodePORTconfig();
            COMPorts();
            DefaultPortValues();
            AddItemsInNodes();
        }

        private void SelectedPort_TextChanged(object sender, EventArgs e)
        {
            if (COM.IsMatch(SelectedPort.Text))
            {
                OkErrorLabel.Text = string.Empty;
                NodesU.Enabled = true;
                GlobalT.NewPortTMP = SelectedPort.SelectedItem.ToString();
                TmpPort = GlobalT.NewPortTMP;

            }
            else
            {
                NodesU.Enabled = false;
                NewPortUSE.Text = string.Empty;
            }
        }


        private void NodesU_TextChanged(object sender, EventArgs e)
        {

            if (NodeS.IsMatch(NodesU.Text))
            {
                GlobalT.NewNodeTMP = NodesU.SelectedItem.ToString();
                TmpNode = GlobalT.NewNodeTMP;
                NewPortUSE.Text = TmpPort;
                NewNodeUSE.Text = TmpNode;
                Save.Enabled = true;
                DisplayNewConfig();
            }
            else
            {
                Save.Enabled = false;
            }
        }

        private void COMPorts()
        {
            SelectedPort.Items.Clear();
            string[] portNames = System.IO.Ports.SerialPort.GetPortNames();
            SelectedPort.Items.Add("");
            foreach (string portName in portNames)
            {
                if (portName != GlobalV.PortCom)
                {
                    SelectedPort.Items.Add(portName);
                }

            }
            if (SelectedPort.Items.Count > 0)
            {
                SelectedPort.SelectedIndex = 0; // Select the first port by default
            }
            else { SelectedPort.Text = "No ports available"; }
        }

        //--- Save Button ---\\\
        private void Save_Click(object sender, EventArgs e)
        {
            if ((COM.IsMatch(SelectedPort.Text)) && (NodeS.IsMatch(NodesU.Text)))
            {
                IniFile ini = new IniFile(NodePort);
                ini.Write("ConfigNodes", "PortCom", TmpPort);
                ini.Write("ConfigNodes", "NodesUse", TmpNode);

                //--- Clear and update window ---\\
                ClearNewConfig();
                DisplayNodePORTconfig();
                AddItemsInNodes();
                COMPorts();
                //--- ---\\
                OkErrorLabel.Text = "Update Complete Port:" + TmpPort;
                OkErrorLabel.ForeColor = Color.Green;
                //--- Save Log ---\\
                LogEvent(TmpPort, TmpNode, GlobalV.LogId, GlobalV.LogUser, Hour, Date);
            }
            else
            {
                OkErrorLabel.Text = "Error action Port not configure";
                OkErrorLabel.ForeColor = Color.Red;
            }


        }

        //--- Display configuration Port & Nodes ---\\
        public void DisplayActualConfigList()
        {
            ConfigList.Clear();
            ConfigList.Text = " Port configurate:" + PortCOM + Environment.NewLine;
            int Count = 1;
            while (Count <= NodeuseToint)
            {

                ConfigList.AppendText("     ●Node-" + Count + Environment.NewLine);
                ConfigList.AppendText("          └Sensor " + Count + "-A" + Environment.NewLine);
                ConfigList.AppendText("          └Sensor " + Count + "-B" + Environment.NewLine);
                Count++;

            }
        }


        /* Create the Port Node LOG
         * -Structure:
         * 
         *┌──────────| Port Node Log |──────────┐
         *│                                     │
         *│● Port: COM11
         *│● Nodes: 29
         *│● User: Key1004098ws
         *│● Name: JICR
         *│● Hour: 11:26 AM
         *│● Date: Wednesday, October 18, 2023
         *└─────────────────────────────────────┘ 
         */
        public static void LogEvent(string Port, string Nodes, string Id, string Owner, string Hour, string Date)
        {

            int LogCount = File.ReadLines(path).Where(line => line.Contains("┌──────────| Port Node Log |──────────┐")).Count();


            if (LogCount >= 300)
            {
                File.WriteAllText(path, string.Empty);
            }

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("┌──────────| Port Node Log |──────────┐");
                writer.WriteLine("│                                     │");
                writer.WriteLine($"│● Port: {Port}");
                writer.WriteLine($"│● Nodes: {Nodes}");
                writer.WriteLine($"│● User: {Id}");
                writer.WriteLine($"│● Name: {Owner}");
                writer.WriteLine($"│● Hour: {Hour}");
                writer.WriteLine($"│● Date: {Date}");
                writer.WriteLine("└─────────────────────────────────────┘");
                writer.WriteLine();
            }
        }
    }
}
