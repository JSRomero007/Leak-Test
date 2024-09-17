/*
 * 
 * 
 * 
 *
 *
 *
 */



using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeakInterface.Global;
using System.Text.RegularExpressions;
using LeakInterface.GlobalIni;
using System.IO;
using System.ComponentModel;
using LeakInterface.CSVRead;
using System.Linq;
using IniParser.Model;
using IniParser;

namespace LeakInterface
{
    public partial class LeakControl : Form
    {

        string xmlFilePath = GlobalL.Csequences;
        //--- Regex variables ---\\
        Regex Zcode = new Regex(@"^Z\d{8}$");
        Regex state = new Regex(@"^(Test|Wait|Start)$");
        Regex Pass = new Regex(@"^(True|False)$");

        private FileSystemWatcher watcher;
        string Ini = GlobalL.WeetechConfiguration;
        public LeakControl()
        {
            InitializeComponent();
            DefaultValues();
        }

        //------------- Important INI reader -------------\\
        //------------------------------------------------\\
        private string ReadValueFromIni(string section, string key)
        {
            string FileIni = Ini;

            int maxAttempts = 5;  // Número de intentos
            int delayBetweenAttempts = 10;  // Retardo en milisegundos entre intentos

            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(FileIni))
                    {
                        string line;
                        bool isInSection = false;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Trim().Equals($"[{section}]"))
                            {
                                isInSection = true;
                                continue;
                            }
                            if (isInSection && line.StartsWith("[") && line.EndsWith("]"))
                            {
                                break;
                            }
                            if (isInSection)
                            {
                                string[] keyValue = line.Split('=');
                                if (keyValue.Length == 2 && keyValue[0].Trim().Equals(key))
                                {
                                    return keyValue[1].Trim();
                                }
                            }
                        }
                    }
                    return null;
                }
                catch (IOException EX)
                {
                    //--- Error if Leak is disable ---\\
                    //--------------------------------\\
                    Console.WriteLine("Data not found");
                    Console.WriteLine(EX);
                    if (i < maxAttempts - 1)
                    {
                        Thread.Sleep(delayBetweenAttempts);
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return null;
        }
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string FileIni = Ini;
            int i = 0;
            watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(FileIni),
                Filter = Path.GetFileName(FileIni),
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (s, ev) =>
            {
                // Read change Ini file
                string Zcode = ReadValueFromIni("Config", "ZCode");
                string State = ReadValueFromIni("Config", "State");
                string Pass = ReadValueFromIni("Config", "Pass");
                string Error = ReadValueFromIni("Config", "Error");
                // Use method Invoke for update
                this.Invoke(new Action(() =>
                {
                    //Save data in Global Varibles
                    GlobalT.T_Zcode = Zcode;
                    GlobalT.T_State = State;
                    GlobalT.T_Pass = Pass;
                    GlobalT.T_Error = Error;

                }));
                i++;
                if (i == 2)
                {
                    IniReader();
                    i = 0;
                }
            };
            watcher.EnableRaisingEvents = true;
            //Open thread
            EventWaitHandle waitHandle = new AutoResetEvent(false);
            waitHandle.WaitOne();
        }
        private void IniReader()
        {
            GlobalT.Error = 0;
            try
            {
                //--- Read only 4 end numbers ---\\
                string ZOnly4 = GlobalT.T_Zcode.Substring(GlobalT.T_Zcode.Length - 4);
                string NewZcode = "ZXXXX" + ZOnly4;
                var MyDic = GlobalData.Data;
                if (MyDic.TryGetValue(NewZcode, out var record))
                {

                    if (GlobalV.StateLeak == 1 && Zcode.IsMatch(GlobalT.T_Zcode) && state.IsMatch(GlobalT.T_State) && Pass.IsMatch(GlobalT.T_Pass) && GlobalT.T_State == "Start")
                    {
                        UpdateUI();
                        Thread thread = new Thread(() =>
                        {
                            Windows._006_Graphic._001_MainPlotter window = new Windows._006_Graphic._001_MainPlotter();
                            window.Closed += (sender, e) => System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Background);
                            GC.Collect();
                            window.Show();
                            System.Windows.Threading.Dispatcher.Run();
                        });
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    }
                }
                else
                {
                    GlobalT.Error = 1;
                }
            }
            catch
            { }
        }

        public void UpdateUI()
        {
            var parser = new FileIniDataParser();
            IniData data = new IniData();

            string ZOnly4 = GlobalT.T_Zcode.Substring(GlobalT.T_Zcode.Length - 4);
            string NewZcode = "ZXXXX" + ZOnly4;
            var MyDic = GlobalData.Data;
            if (xmlFilePath != "")
            {

                if (MyDic.TryGetValue(NewZcode, out var record))
                {
                    GlobalT.Sequence = string.Join(", ", record.Select(kvp => $"{kvp.Value}"));
                    string[] SequenceSave = GlobalT.Sequence.Split(',');

                    try
                    {
                        //--- SequenceUsed ---\\
                        if (OnOffbtn.Checked)
                        {
                            data["Config"]["Autoleak"] = "Enable";
                        }
                        else { data["Config"]["Autoleak"] = "Disable"; }
                        //--- Name Sequence ---\\
                        data["Config"]["NameSec"] = SequenceSave[5].ToString();
                        GlobalT.T_NameSec = SequenceSave[5].ToString();
                        //--- ID sequence ---\\
                        data["Config"]["IdSec"] = SequenceSave[0].ToString();
                        GlobalT.T_IDsec = SequenceSave[0].ToString();
                        //--- Z Code ---\\
                        data["Config"]["Zcode"] = GlobalT.T_Zcode;
                        GlobalT.T_Zcod = SequenceSave[3].ToString();
                        //--- Programmer ---\\
                        data["Config"]["Programmer"] = SequenceSave[1].ToString();
                        GlobalT.T_Programmer = SequenceSave[1].ToString();
                        //--- Date Sequence ---\\
                        data["Config"]["DateSec"] = SequenceSave[2].ToString();
                        GlobalT.T_Datesec = SequenceSave[2].ToString();
                        //--- Version ---\\
                        data["Config"]["Version"] = SequenceSave[4].ToString();
                        GlobalT.T_version = SequenceSave[4].ToString();
                        //--- Node num ---\\
                        data["Config"]["NodeNum"] = SequenceSave[7].ToString();
                        GlobalT.T_NodeNum = SequenceSave[7].ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Data not found ");
                        MessageBox.Show("Error:------ " + ex.Message, "Error++++++++", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Console.WriteLine("ZXXXX5111 no encontrado.");
                }
            }
            parser.WriteFile(GlobalL.ASequence, data);
        }
        //------------------------------------------------------//
        private readonly string iniPath2 = GlobalL.ASequence;
        private FileSystemWatcher Iwatcher;
        private IniReader iniReader;
        private void InitializeWatcher()
        {
            Iwatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(iniPath2),
                Filter = Path.GetFileName(iniPath2),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime
            };
            Iwatcher.Changed += OnFileChanged;
            Iwatcher.Created += OnFileChanged;
            Iwatcher.Renamed += OnFileChanged;
            Iwatcher.EnableRaisingEvents = true;
        }

        private async void OnFileChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                await Task.Delay(500);
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(UpdateLabelContent));
                }
                else
                {
                    UpdateLabelContent();
                }
            }
            catch (Exception ex)
            {
                // Gestionar o loguear excepciones aquí
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateLabelContent()
        {

            try
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(GlobalL.ASequence);
                string value = data["Config"]["Autoleak"];

                if (value == "Enable")
                { OnOffbtn.Checked = true; }
                else if (value == "Disable")
                { OnOffbtn.Checked = false; }
                else { OnOffbtn.Checked = false; }

                lIDsequence.Text = data["Config"]["IdSec"]; ;
                lNameSequence.Text = data["Config"]["NameSec"]; ;
                lZcode.Text = data["Config"]["Zcode"]; ;
                lProgrammer.Text = data["Config"]["Programmer"]; ;
                lDate.Text = data["Config"]["DateSec"]; ;
                lVersion.Text = data["Config"]["Version"]; ;
                lNodeNum.Text = data["Config"]["NodeNum"]; ;


            }
            catch (Exception ex)
            {
                Console.WriteLine("DATA not found: " + ex);
            }

        }

        //------------------------------------------------------//
        //---------XML Write ----------//

        private void AutoLoad()
        {
            //CenterWindow();
            GC.Collect();
            labelV.Text = GlobalV.Version.ToString();
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerAsync();
            iniReader = new IniReader(iniPath2);
            InitializeWatcher();
            UpdateLabelContent();

        }

        //--------- Window State ----------//
        private void WindowState()
        {
            this.Hide();
            PopState form2 = new PopState();
            form2.ShowDialog();
            this.Show();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            WindowState();

        }
        private void LeakControl_Load(object sender, EventArgs e)
        {

            GC.Collect();
            AutoLoad();
            UpdateLabelContent();


            string State = iniReader.GetValue("Config", "Autoleak");
            if (State == "Enable")
            { OnOffbtn.Checked = true; }
            else if (State == "Disable")
            { OnOffbtn.Checked = false; }
            else { OnOffbtn.Checked = false; }

        }
        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            GC.Collect();
            this.Hide();
            User Next_User = new User();
            Next_User.ShowDialog();
            this.Show();
        }
        private void OnOffbtnstate()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(GlobalL.ASequence);

            if (OnOffbtn != null)
            {
                if (GlobalV.StateLeak == 1)
                {
                    GlobalV.StateLeak = 0;
                    //--- Disble Auto Leak ---\\
                    DisableLeakL.Visible = true;
                    DisableLeakP.Visible = true;
                    Console.WriteLine("Disable");
                    data["Config"]["Autoleak"] = "Disable";
                    Console.WriteLine($"Después de editar: {data}");
                    parser.WriteFile(GlobalL.ASequence, data);
                }
                else
                {
                    GlobalV.StateLeak = 1;
                    //--- Enable Auto Leak ---\\
                    DisableLeakP.Visible = false;
                    DisableLeakL.Visible = false;
                    Console.WriteLine("Enable");
                    data["Config"]["Autoleak"] = "Enable";
                    Console.WriteLine($"Después de editar: {data}");
                    parser.WriteFile(GlobalL.ASequence, data);
                }


            }
        }
        private void OnOffbtn_CheckedChanged(object sender, EventArgs e)
        {

            OnOffbtnstate();
        }
        //--- Restart Window ---\\
        private void pictureBox1_Click(object sender, EventArgs e)
        { GC.Collect(); VisibleClosedWindow(); }

        private void DefaultValues()
        {
            ErrorCode.Text = string.Empty;
            BClosingWindow.Visible = false;
            FClosingWindow.Visible = false;
        }
        private void VisibleClosedWindow()
        {

            FClosingWindow.Visible = true;
            BClosingWindow.Visible = true;

        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            string code = ExitCode.Text;
            if (code == "Aptiv711")
            {
                Application.Exit();
            }
            else
            {
                ErrorCode.Text = "Incorrect code";
                ErrorCode.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            DefaultValues();
        }
        private void CenterWindow()
        {
            Screen[] screens = Screen.AllScreens;

            // Seleccionar una pantalla específica (en este caso, la primera pantalla)
            // Puedes cambiar el índice para seleccionar una pantalla diferente
            Screen myScreen = screens[GlobalV.SPosition];

            // Configurar la posición de la ventana principal para que se muestre en la pantalla seleccionada
            // Puedes ajustar los valores para cambiar la posición dentro de la pantalla
            this.Left = myScreen.Bounds.Left;
            this.Top = myScreen.Bounds.Top;
            int x = myScreen.Bounds.X + (myScreen.Bounds.Width - this.Width) / 2;
            int y = myScreen.Bounds.Y + (myScreen.Bounds.Height - this.Height) / 2;

            // Configurar la posición de la ventana principal para que se muestre en el centro de la pantalla seleccionada
            this.Location = new System.Drawing.Point(x, y);
        }
    }
}
