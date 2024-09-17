using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using LeakInterface.Global;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace LeakInterface.Windows._003_Sequence
{
    public partial class _002_AddEditDeleteSequences : Form
    {
        string FullRegexString = @"^[A-Za-z]+-\d{1,2}\s(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|[1-9])\s(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|[1-9])\s(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|[1-9])\s(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|[1-9])\s(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|[1-9])\s(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|[1-9])$";
        int ZError = 0;
        string CompareDS1;
        string CompareDS2;
        string CsvPath = GlobalL.DBSequence;
        string NodePort = GlobalL.PortConfig;
        string DefaultCard = GlobalL.DefaultCardConfig;
        private HashSet<string> keys = new HashSet<string>();
        private List<TextBox> LS1Textbox = new List<TextBox>();
        private List<TextBox> LS2Textbox = new List<TextBox>();
        private RadioButton selectedRadioButton;
        private SaveSequences registro = new SaveSequences();
        private DateTime now = DateTime.Now;
        private static StringBuilder concatenatedValues = new StringBuilder();
        private SaveSequences saveSequences = new SaveSequences();
        private string ExtractZcode;
        public static string CardNumber;
        bool ViewADD, ViewUpdate, ViewClear;

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
        }//Only read and write Ini File //
        public _002_AddEditDeleteSequences()
        {
            InitializeComponent();
            LoadInfoSequences();
            ClearElements();
            ReadActualPortConfig();
            Disablecaptions();
            Search_ZCode.TextChanged += Search_ZCode_TextChanged;
            saveSequences.OnNewSequenceAdded += SaveSequences_OnNewSequenceAdded;
            Search_ZCode.Text = "";
            Sensor1(); Sensor2();
            REGeventTextBox();
            ReadActualCardConfig();
        }

        private void Disablecaptions()
        {
            NameSequence.Enabled = false;
            Version.Enabled = false;
            flowLayoutPanel1.Enabled = false;
            // Cards configuration \\
            Sensor1State("Disable");
            Sensor2State("Disable");
            ZCode_Sequence.TextChanged += (sender, e) => ValidateTextBox(ZCode_Sequence, NameSequence, @"^\d{4}$");
            NameSequence.TextChanged += (sender, e) => ValidateTextBox(NameSequence, Version, @"^.{3,40}$");
            Version.TextChanged += Version_TextChanged;
            //NameSequence_TextChanged +=(sender, e)=>ValidateTexBox(NameSequence,)
        }
        private void ValidateTextBox(TextBox currentTextBox, TextBox nextTextBox, string regex, params TextBox[] subsequentTextBoxes)
        {
            bool isValid = Regex.IsMatch(currentTextBox.Text, regex);

            nextTextBox.Enabled = isValid;
            if (!isValid)
            {
                // if regex is match enable the next textbox.
                foreach (var textBox in subsequentTextBoxes)
                {
                    textBox.Enabled = false;
                }
            }
            else
            {
                if (subsequentTextBoxes.Length > 0 && nextTextBox == subsequentTextBoxes[0])
                {
                }
            }
        }

        private void LoadInfoSequences()
        {
            try
            {
                DataTable DT = new DataTable();
                using (StreamReader reader = new StreamReader(CsvPath))
                {
                    string line;
                    bool isHeader = true;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',');

                        if (isHeader)
                        {
                            // Asumiendo que siempre habrá al menos 7 columnas (índices del 0 al 6)
                            DT.Columns.Add(fields[4]);
                            DT.Columns.Add(fields[6]);
                            DT.Columns.Add(fields[5]);
                            isHeader = false;
                        }
                        else
                        {
                            // Verificar si alguno de los campos relevantes está vacío
                            if (string.IsNullOrWhiteSpace(fields[4]) ||
                                string.IsNullOrWhiteSpace(fields[6]) ||
                                string.IsNullOrWhiteSpace(fields[5]))
                            {
                                // Si algún campo está vacío, omitimos esta fila
                                continue;
                            }
                            // Si todos los campos relevantes tienen contenido, añadimos la fila
                            DT.Rows.Add(fields[4], fields[6], fields[5]);
                        }
                    }
                }
                DataViewCSV.DataSource = DT;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void DisplaySelectionSequences(string SearchData)
        {
            if (DataViewCSV.SelectedRows.Count > 0)
            {
                string Search = DataViewCSV.SelectedRows[0].Cells[0].Value.ToString();
                string[] csvData = File.ReadAllLines(CsvPath);
                foreach (string row in csvData)
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        string[] rowData = row.Split(',');
                        if (rowData.Length >= 10 && rowData[4] == Search && SearchData == null)
                        {
                            //Console.WriteLine(rowData[4]);
                            GlobalV.ExtractZcode = rowData[4].Replace("ZXXXX", "").Trim();
                            Search_ZCode.Text = GlobalV.ExtractZcode; // Por ejemplo, muestra la información de la Columna6
                            break;
                        }
                        if (rowData.Length >= 10 && rowData[4] == SearchData)
                        {
                            ZError = 0;
                            //Console.WriteLine("THIS IS CORRECT");
                            GlobalV.ExtractVersion = rowData[5];
                            GlobalV.ExtractNameSequence = rowData[6];
                            string holderNum = rowData[8];
                            GlobalV.ExtractSequences = rowData[9];
                            if (string.IsNullOrEmpty(SearchData))
                            {
                                Search_ZCode.Text = GlobalV.ExtractZcode;
                                ClearElements();
                            }
                            else
                            {
                                SelectionZcode.Text = GlobalV.ExtractZcode;
                                SelectionVersion.Text = GlobalV.ExtractVersion;
                                SelectionName.Text = GlobalV.ExtractNameSequence;
                                SelectionHolders.Text = holderNum;
                                SelectionSequence.Text = GlobalV.ExtractSequences;
                                EditDeleteButtons.Visible = true;
                            }
                            break;
                        }
                        else
                        {
                            ZError = 1;
                            ClearElements();
                        }
                    }
                }
            }
            AddSequence.Visible = false;
        }/*Search and Select Data in DataView*/

        private void DataViewCSV_SelectionChanged(object sender, EventArgs e)
        {
            DisplaySelectionSequences(null);
        }

        private void Search_ZCode_TextChanged(object sender, EventArgs e)
        {
            CompareCSVData();
        }
        private void NameSequence_TextChanged(object sender, EventArgs e)
        {
            GlobalV.Sequence_Name = NameSequence.Text;
            CorrectOrNotName();
        }
        private void Version_TextChanged(object sender, EventArgs e)
        {
            GlobalV.Sequence_Version = Version.Text;
            CorrectOrNotVersion();
        }
        private void ZCode_Sequence_TextChanged(object sender, EventArgs e)
        {
            GlobalV.Sequence_ZCode = ZCode_Sequence.Text;
            CorrectOrNotZcode();
        }

        private void _002_AddEditDeleteSequences_Load(object sender, EventArgs e)
        {
            LoadFuntion();
        }

        private void UpdateDatagridview()
        {
            LoadInfoSequences();
        }

        private void LoadFuntion()
        {
            CreateRadiobuttonTable();
            keys = ExtractKeysFromColumn4(GlobalL.DBSequence);
        }
        //---- Complement funtions ----\\
        private void CorrectOrNotZcode()
        {
            if (ZCode_Sequence.TextLength >= 4)
            {
                if (Regex.IsMatch(ZCode_Sequence.Text, @"^\d{4}$"))
                {
                    ErrorZcode.Text = string.Empty;
                }
                else
                {
                    ErrorZcode.ForeColor = Color.Red;
                    ErrorZcode.Text = "Ex.: '1212' - '1546' ";
                }
            }
        }
        private void CorrectOrNotVersion()
        {
            if (Regex.IsMatch(Version.Text, @"^\d+(\.\d+)+$"))
            {
                ErrorVersion.Text = string.Empty;
                flowLayoutPanel1.Enabled = true;
            }
            else if (Version.Text == string.Empty)
            {
                flowLayoutPanel1.Enabled = false;
                Sensor1State("Disable");
                Sensor2State("Disable");
                ErrorVersion.Text = string.Empty;
            }
            else
            {
                flowLayoutPanel1.Enabled = false;
                Sensor1State("Disabel");
                Sensor2State("Disabel");
                ErrorVersion.Text = "Ex.: 1.1 or 1.583.6";
                ErrorVersion.ForeColor = Color.Red;
            }
        }
        private void CorrectOrNotName()
        {
            if (Regex.IsMatch(NameSequence.Text, @"^.{3,40}$"))
            {
                ErrorNameSequence.Text = string.Empty;
            }
            else if (NameSequence.Text == string.Empty)
            {
                ErrorNameSequence.Text = string.Empty;
            }
            else
            {
                ErrorNameSequence.Text = "Ex. (Name): Ford-45a6 ";
                ErrorNameSequence.ForeColor = Color.Red;
            }
        }
        private void CorrectOrNotSequence()
        {
            string search = Search_ZCode.Text.ToLower().Trim(); // Utiliza Trim() para eliminar espacios en blanco al inicio y al final
            if (search.Length >= 4)
            {
                // Preparar el ZCODE completo
                string ZCODE = "ZXXXX" + search;
                DisplaySelectionSequences(ZCODE); // Asegúrate de que esta función esté implementada correctamente
                if (Regex.IsMatch(search, @"^\d{4}$") && ZError == 0) // Asegúrate de que ZError esté definido y actualizado correctamente
                {
                    ErrorSearchZ.Text = string.Empty;
                }
                else
                {
                    ZCode_Sequence.Text = search;
                    ErrorSearchZ.ForeColor = Color.Red;
                    ErrorSearchZ.Text = "Zcode not found Ex.: '1212' - '1546' or Add Sequence";
                }
            }
            else if (search == string.Empty)
            {

            }
        }

        private void CompareCSVData()
        {
            bool DicNull = registro.Sequence.All(kv => string.IsNullOrEmpty(kv.Value));
            CorrectOrNotSequence();
            ExtractZcode = Search_ZCode.Text;
            if (keys.Contains("ZXXXX" + Search_ZCode.Text) && Regex.IsMatch(ExtractZcode, @"^\d{4}$"))
            {
                ErrorSearchZ.ForeColor = Color.Green;
                ErrorZcode.ForeColor = Color.Green;
                ErrorSearchZ.Text = "Existing sequence →.";
            }
            else if (!keys.Contains("ZXXXX" + Search_ZCode.Text) && Search_ZCode.TextLength > 3 && Regex.IsMatch(ExtractZcode, @"^\d{4}$"))
            {
                ClearAlltoAdd();
                ErrorSearchZ.ForeColor = Color.Green;
                ErrorZcode.ForeColor = Color.Green;
                ErrorSearchZ.Text = "Zcode not found, Add Sequence ↑.";
                ErrorZcode.Text = "New Zcode ZXXXX" + Search_ZCode.Text;
                ViewADD = true;
            }
            else if (Search_ZCode.TextLength > 3 && !Regex.IsMatch(ExtractZcode, @"^\d{4}$"))
            {
                Clearall();
                ViewADD = false;
                ViewClear = false;
                ViewUpdate = false;
                ErrorSearchZ.ForeColor = Color.Red;
                ErrorSearchZ.Text = "Incorrect format Ex.: '1212' - '1546'";
            }
            else
            {
                Clearall();
                ViewADD = false;
                ViewClear = false;
                ViewUpdate = false;
                ZCode_Sequence.Text = "";
                ErrorSearchZ.Text = "";
            }
            AddSequence.Visible = ViewADD;
            ClearrSequence.Visible = ViewClear;
            UpdateSequence.Visible = ViewUpdate;
        }

        //--- Main funtions ---//
        //--- Ini Read ---\\
        private void ReadActualPortConfig()
        {
            //--- Save string in GlobalV ---\\
            IniFile ini = new IniFile(NodePort);
            GlobalV.PortCom = (ini.Read("ConfigNodes", "PortCom"));
            GlobalV.NodesUse = (ini.Read("ConfigNodes", "NodesUse"));
            if (GlobalV.PortCom != "" && GlobalV.NodesUse != "")
            {
                PortLabel.Text = GlobalV.PortCom;
                MinCard.Text = "1";
                MaxCards.Text = GlobalV.NodesUse;
                //Console.WriteLine(GlobalV.PortCom);
            }
            else
            {
                PortLabel.Text = "Error in Port";
            }
            for (int i = 1; i <= int.Parse(GlobalV.NodesUse); i++)
            {
                // Generando las claves según el formato deseado y agregándolas al diccionario
                registro.AgregarOActualizar(i + "S1", "");
                registro.AgregarOActualizar(i + "S2", "");
            }

        }
        private void ReadActualCardConfig()
        {    //--- Save string in GlobalV ---\\
            IniFile ini = new IniFile(DefaultCard);
            //--- Sensor 1 ---\\
            GlobalV.Dweel_S1 = (ini.Read("CardConfig", "Dweel_S1"));
            GlobalV.Threshold_S1 = (ini.Read("CardConfig", "Threshold_S1"));
            GlobalV.Settle_S1 = (ini.Read("CardConfig", "Settle_S1 "));
            GlobalV.Min_S1 = (ini.Read("CardConfig", "Min_S1 "));
            GlobalV.Time_S1 = (ini.Read("CardConfig", "Time_S1 "));
            GlobalV.Final_S1 = (ini.Read("CardConfig", "Final_S1 "));
            //--- Sensor 2 ---\\
            GlobalV.Dweel_S2 = (ini.Read("CardConfig", "Dweel_S1"));
            GlobalV.Threshold_S2 = (ini.Read("CardConfig", "Threshold_S1"));
            GlobalV.Settle_S2 = (ini.Read("CardConfig", "Settle_S1 "));
            GlobalV.Min_S2 = (ini.Read("CardConfig", "Min_S1 "));
            GlobalV.Time_S2 = (ini.Read("CardConfig", "Time_S1 "));
            GlobalV.Final_S2 = (ini.Read("CardConfig", "Final_S1 "));

        }
        public bool Default_S1()
        {       // Prepara el diccionario con los valores de TextBox hasta "Final_S1"
            Dictionary<string, string> valoresTextBox = new Dictionary<string, string>()
            {
                {"Dweel_S1", Dwell_S1.Text.Trim()},
                {"Threshold_S1", Threshold_S1.Text.Trim()},
                {"Settle_S1", Settle_S1.Text.Trim()},
                {"Min_S1", Min_S1.Text.Trim()},
                {"Time_S1", ThreshTime_S1.Text.Trim()},
                {"Final_S1", Final_S1.Text.Trim()},
            };

            // Prepara el diccionario con los valores de GlobalV hasta "Final_S1"
            Dictionary<string, string> valoresGlobalV = new Dictionary<string, string>()
            {
                {"Dweel_S1", GlobalV.Dweel_S1.Trim()},
                {"Threshold_S1", GlobalV.Threshold_S1.Trim()},
                {"Settle_S1", GlobalV.Settle_S1.Trim()},
                {"Min_S1", GlobalV.Min_S1.Trim()},
                {"Time_S1", GlobalV.Time_S1.Trim()},
                {"Final_S1", GlobalV.Final_S1.Trim()},
            };

            // Compara los valores de los diccionarios
            foreach (var item in valoresTextBox)
            {
                if (!valoresGlobalV.ContainsKey(item.Key) || valoresGlobalV[item.Key] != item.Value)
                {
                    return false; // Si algún valor no coincide
                }
            }
            return true; // Si todos los valores coinciden
        }
        public bool Default_S2()
        {       // Prepara el diccionario con los valores de TextBox hasta "Final_S1"
            Dictionary<string, string> valoresTextBox = new Dictionary<string, string>()
            {
                {"Dweel_S1", Dwell_S2.Text.Trim()},
                {"Threshold_S1", Threshold_S2.Text.Trim()},
                {"Settle_S1", Settle_S2.Text.Trim()},
                {"Min_S1", Min_S2.Text.Trim()},
                {"Time_S1", ThreshTime_S2.Text.Trim()},
                {"Final_S1", Final_S2.Text.Trim()},
            };

            // Prepara el diccionario con los valores de GlobalV hasta "Final_S1"
            Dictionary<string, string> valoresGlobalV = new Dictionary<string, string>()
            {
                {"Dweel_S1", GlobalV.Dweel_S2.Trim()},
                {"Threshold_S1", GlobalV.Threshold_S2.Trim()},
                {"Settle_S1", GlobalV.Settle_S2.Trim()},
                {"Min_S1", GlobalV.Min_S2.Trim()},
                {"Time_S1", GlobalV.Time_S2.Trim()},
                {"Final_S1", GlobalV.Final_S2.Trim()},
            };

            // Compara los valores de los diccionarios
            foreach (var item in valoresTextBox)
            {
                if (!valoresGlobalV.ContainsKey(item.Key) || valoresGlobalV[item.Key] != item.Value)
                {
                    return false; // Si algún valor no coincide
                }
            }
            return true; // Si todos los valores coinciden
        }

        //-------------- Checkbox Zone --------------\\
        private void DefaultValues_1_CheckedChanged(object sender, EventArgs e)
        {

            if (DefaultValues_1.Checked == true)
            {
                ConfigSensor1("Disable");
                DataSensor1("DefaultValues");
            }
            else
            {
                ConfigSensor1("Enable");
                DataSensor1("Clear");
            }
        }
        private void DefaultValues_2_CheckedChanged(object sender, EventArgs e)
        {

            if (DefaultValues_2.Checked == true)
            {
                ConfigSensor2("Disable");
                DataSensor2("DefaultValues");
            }
            else
            {
                ConfigSensor2("Enable");
                DataSensor2("Clear");
            }



        }


        //-------------- Textbox Zone (S1 & S2) --------------\\
        private void Dwell_S1_TextChanged_1(object sender, EventArgs e)
        { ConfirmDataS1("Enable"); }
        private void Threshold_S1_TextChanged(object sender, EventArgs e)
        { ConfirmDataS1("Enable"); }
        private void Settle_S1_TextChanged(object sender, EventArgs e)
        { ConfirmDataS1("Enable"); }
        private void Min_S1_TextChanged(object sender, EventArgs e)
        { ConfirmDataS1("Enable"); }
        private void ThreshTime_S1_TextChanged(object sender, EventArgs e)
        { ConfirmDataS1("Enable"); }
        private void Final_S1_TextChanged(object sender, EventArgs e)
        { ConfirmDataS1("Enable"); }
        private void Dwell_S2_TextChanged(object sender, EventArgs e)
        { ConfirmDataS2("Enable"); }
        private void Threshold_S2_TextChanged(object sender, EventArgs e)
        { ConfirmDataS2("Enable"); }
        private void Settle_S2_TextChanged(object sender, EventArgs e)
        { ConfirmDataS2("Enable"); }
        private void Min_S2_TextChanged(object sender, EventArgs e)
        { ConfirmDataS2("Enable"); }
        private void ThreshTime_S2_TextChanged(object sender, EventArgs e)
        { ConfirmDataS2("Enable"); }
        private void Final_S2_TextChanged(object sender, EventArgs e)
        { ConfirmDataS2("Enable"); }

        //-------------- Sensor Zone --------------\\
        private void Sensor1State(string state)
        {
            if (state == "Enable")
            {
                Name_S1.Enabled = true;
            }
            else
            {
                Name_S1.Enabled = false;
            }
            StatusS1();
        }
        private void ConfigSensor1(String Mode)
        {
            if (Mode == "Enable")
            {
                Dwell_S1.Enabled = true;
                Threshold_S1.Enabled = true;
                Settle_S1.Enabled = true;
                Min_S1.Enabled = true;
                ThreshTime_S1.Enabled = true;
                Final_S1.Enabled = true;
            }
            else
            {
                Dwell_S1.Enabled = false;
                Threshold_S1.Enabled = false;
                Settle_S1.Enabled = false;
                Min_S1.Enabled = false;
                ThreshTime_S1.Enabled = false;
                Final_S1.Enabled = false;
            }
        }
        private void DataSensor1(String Mode)
        {
            if (Mode == "DefaultValues")
            {
                Dwell_S1.Text = GlobalV.Dweel_S1;
                Threshold_S1.Text = GlobalV.Threshold_S1;
                Settle_S1.Text = GlobalV.Settle_S1;
                Min_S1.Text = GlobalV.Min_S1;
                ThreshTime_S1.Text = GlobalV.Time_S1;
                Final_S1.Text = GlobalV.Final_S1;
            }
            else
            {
                Dwell_S1.Text = string.Empty;
                Threshold_S1.Text = string.Empty;
                Settle_S1.Text = string.Empty;
                Min_S1.Text = string.Empty;
                ThreshTime_S1.Text = string.Empty;
                Final_S1.Text = string.Empty;
            }
        }
        private void Sensor2State(string state)
        {
            if (state == "Enable")
            {
                Name_S2.Enabled = true;
            }
            else
            {
                Name_S2.Enabled = false;
            }
            StatusS2();
        }
        private void ConfigSensor2(String Mode)
        {
            if (Mode == "Enable")
            {
                Dwell_S2.Enabled = true;
                Threshold_S2.Enabled = true;
                Settle_S2.Enabled = true;
                Min_S2.Enabled = true;
                ThreshTime_S2.Enabled = true;
                Final_S2.Enabled = true;
            }
            else
            {
                Dwell_S2.Enabled = false;
                Threshold_S2.Enabled = false;
                Settle_S2.Enabled = false;
                Min_S2.Enabled = false;
                ThreshTime_S2.Enabled = false;
                Final_S2.Enabled = false;
            }


        }
        private void DataSensor2(String Mode)
        {
            if (Mode == "DefaultValues")
            {
                Dwell_S2.Text = GlobalV.Dweel_S2;
                Threshold_S2.Text = GlobalV.Threshold_S2;
                Settle_S2.Text = GlobalV.Settle_S2;
                Min_S2.Text = GlobalV.Min_S2;
                ThreshTime_S2.Text = GlobalV.Time_S2;
                Final_S2.Text = GlobalV.Final_S2;
            }
            else
            {
                Dwell_S2.Text = string.Empty;
                Threshold_S2.Text = string.Empty;
                Settle_S2.Text = string.Empty;
                Min_S2.Text = string.Empty;
                ThreshTime_S2.Text = string.Empty;
                Final_S2.Text = string.Empty;
            }
        }
        //-----------------------------------------\\

        //---------------- Buttons ----------------\\
        private void AddSequence_Click(object sender, EventArgs e)
        {

            try
            {
                bool addCsvResult = AddCSV();
                if (addCsvResult)
                {
                    AddSequence.Visible = !addCsvResult;
                    UpdateSequence.Visible = addCsvResult;
                    UpdateSequence.Enabled = !addCsvResult;
                    ClearrSequence.Visible = addCsvResult;
                    ClearrSequence.Enabled = !addCsvResult;
                }
            }
            catch (Exception ex) { ErrorSequence.Text = "Error in Update" + ex; }
        }
        private void EditSequence_Click(object sender, EventArgs e)
        {
            EditSeq();
        }
        private void UpdateSequence_Click(object sender, EventArgs e)
        {
            try
            {
                bool UpdateResult = UpdateCsv();
                if (UpdateResult)
                {
                    //UpdateSequence.Visible = UpdateCsv;
                    UpdateSequence.Enabled = !UpdateResult;
                    //ClearrSequence.Visible = UpdateCsv;
                    ClearrSequence.Enabled = UpdateResult;
                }
            }
            catch (Exception ex) { ErrorSequence.Text = "Error in Update" + ex; }
        }
        private void ClearSequence_Click(object sender, EventArgs e)
        {
            registro.DeleteSequence();
            UpdateRadioButton();
        }
        private void DeleteSequence_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteCsv();
            }
            catch (Exception ex) { ErrorSequence.Text = "Error in Update" + ex; }
        }
        //-----------------------------------------\\

        private void ClearElements()
        {
            ZCode_Sequence.Enabled = false;
            SelectionZcode.Text = String.Empty;
            SelectionVersion.Text = String.Empty;
            SelectionName.Text = String.Empty;
            SelectionHolders.Text = String.Empty;
            SelectionSequence.Text = String.Empty;

            ErrorZcode.Text = String.Empty;
            ErrorNameSequence.Text = String.Empty;
            ErrorVersion.Text = String.Empty;
            ErrorSequence.Text = String.Empty;

            AutoSaveS1.Text = String.Empty;
            AutoSaveS2.Text = String.Empty;
            ErrorSearchZ.Text = string.Empty;

            //DeleteSequence.Visible = false;
            ClearrSequence.Visible = false;
            UpdateSequence.Visible = false;
            AddSequence.Visible = false;

            EditDeleteButtons.Visible = false;

        }/*Clear Elements all elements*/
        private void ClearConfig()
        {
            if (selectedRadioButton != null)
            {
                if (selectedRadioButton.Text.EndsWith("(Configurated)"))
                {
                    selectedRadioButton.Text = selectedRadioButton.Text.Replace(" (Configurated)", "");
                }
            }
        }
        private void Clearall()
        {

            ClearConfig();
            registro.DeleteSequence();
            UpdateRadioButton();
            ClearSequence();
        }
        private void ClearAlltoAdd()
        {
            ClearConfig();
            registro.DeleteSequence();
            UpdateRadioButton();
            ClearSequenceAndAdd();
        }
        private void ClearSequence()
        {
            isAutomaticChange = false;
            Name_S1.Text = string.Empty;
            DataSensor1("Disable");
            Name_S2.Text = string.Empty;
            DataSensor2("Disable");
            Version.Text = string.Empty;
            NameSequence.Text = string.Empty;
            ZCode_Sequence.Text = string.Empty;
        }
        private void ClearSequenceAndAdd()
        {
            isAutomaticChange = false;
            Name_S1.Text = string.Empty;
            DataSensor1("Disable");
            Name_S2.Text = string.Empty;
            DataSensor2("Disable");
            Version.Text = string.Empty;
            NameSequence.Text = string.Empty;

        }

        private bool NoData(string Data)
        {
            return !Regex.IsMatch(Data, @"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|[1-9])$");
        }

        private void ConfirmDataS1(string state)
        {
            if (state == "Enable")
            {
                var controls = new List<(TextBox textBox, Control visibilityControl)>
             {
                 (Dwell_S1, ND_Dweel_S1),
                 (Threshold_S1, ND_Threshold_S1),
                 (Settle_S1, ND_Settle_S1),
                 (Final_S1, ND_Final_S1),
                 (ThreshTime_S1, ND_Time_S1),
                 (Min_S1, ND_Min_S1)
             };

                bool allTextBoxesFilled = true; // Asumimos que todos los TextBox están llenos inicialmente

                foreach (var (textBox, visibilityControl) in controls)
                {
                    if (!textBox.Enabled)
                    {
                        visibilityControl.Visible = false;
                    }
                    else
                    {
                        bool noData = NoData(textBox.Text);
                        visibilityControl.Visible = noData;

                        // Si alguno de los TextBox está vacío (o cumple la condición de NoData), actualizamos el flag
                        if (noData)
                        {
                            allTextBoxesFilled = false;
                        }
                    }
                }

                // Si todos los TextBox están llenos, no mostramos el mensaje. De lo contrario, mostramos el mensaje.
                if (allTextBoxesFilled)
                {
                    AutoSaveS1.Text = "";
                }
                else
                {
                    AutoSaveS1.Text = "Complet box with *";
                    AutoSaveS1.ForeColor = Color.Red;
                }
            }
            else
            {
                AutoSaveS1.Text = string.Empty;
            }
        }
        private void ConfirmDataS2(string state)
        {
            if (state == "Enable")
            {
                var controls = new List<(TextBox textBox, Control visibilityControl)>
             {
                 (Dwell_S2, ND_Dweel_S2),
                 (Threshold_S2, ND_Threshold_S2 ),
                 (Settle_S2, ND_Settle_S2),
                 (Final_S2, ND_Final_S2 ),
                 (ThreshTime_S2, ND_Time_S2 ),
                 (Min_S2, ND_Min_S2 )
             };
                bool allTextBoxesFilled = true; // Asumimos que todos los TextBox están llenos inicialmente

                foreach (var (textBox, visibilityControl) in controls)
                {
                    if (!textBox.Enabled)
                    {
                        visibilityControl.Visible = false;
                    }
                    else
                    {
                        bool noData = NoData(textBox.Text);
                        visibilityControl.Visible = noData;

                        // Si alguno de los TextBox está vacío (o cumple la condición de NoData), actualizamos el flag
                        if (noData)
                        {
                            allTextBoxesFilled = false;
                        }
                    }
                }

                // Si todos los TextBox están llenos, no mostramos el mensaje. De lo contrario, mostramos el mensaje.
                if (allTextBoxesFilled)
                {
                    AutoSaveS2.Text = "";
                }
                else
                {
                    AutoSaveS2.Text = "Complet box with *";
                    AutoSaveS2.ForeColor = Color.Red;
                }
            }
            else
            {
                AutoSaveS2.Text = string.Empty;
            }
        }

        private void Sensor1()
        {
            LS1Textbox.Add(Name_S1);
            LS1Textbox.Add(Dwell_S1);
            LS1Textbox.Add(Threshold_S1);
            LS1Textbox.Add(Settle_S1);
            LS1Textbox.Add(Min_S1);
            LS1Textbox.Add(ThreshTime_S1);
            LS1Textbox.Add(Final_S1);
        }
        private void Sensor2()
        {
            LS2Textbox.Add(Name_S2);
            LS2Textbox.Add(Dwell_S2);
            LS2Textbox.Add(Threshold_S2);
            LS2Textbox.Add(Settle_S2);
            LS2Textbox.Add(Min_S2);
            LS2Textbox.Add(ThreshTime_S2);
            LS2Textbox.Add(Final_S2);
        }

        // Registrar el evento TextChanged para cada TextBox
        private void REGeventTextBox()
        {
            foreach (var textBox in LS1Textbox)
            {
                // Registra el evento TextChanged en lugar de Leave
                textBox.TextChanged += TextBox_TextChanged;
            }
            foreach (var textBox in LS2Textbox)
            {
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            SaveCompleteSensor(LS1Textbox, "S1", LS2Textbox, "S2");
        }

        private void SaveCompleteSensor(List<TextBox> S1, string ID1, List<TextBox> S2, string ID2)
        {
            string DataSensor1 = DefaultZero(S1);
            string DataSensor2 = DefaultZero(S2);

            // Sensor 1
            if (!string.IsNullOrWhiteSpace(DataSensor1) && ID1 == "S1" && Regex.IsMatch(DataSensor1, FullRegexString) && DataSensor1 != CompareDS1)
            {
                registro.AgregarOActualizar(CardNumber + ID1, DataSensor1);
                CompareDS1 = DataSensor1;
                AutoSaveS1.Text = "Auto Save ✓ (Sensor 1)";
                AutoSaveS1.ForeColor = Color.Green;

                if (Default_S1())
                {
                    ConfigSensor1("Disable");
                    DefaultValues_1.Checked = true;
                    UpdateSequence.Enabled = true;
                }
            }
            else
            {
            }

            // Sensor 2
            if (!string.IsNullOrWhiteSpace(DataSensor2) && ID2 == "S2" && Regex.IsMatch(DataSensor2, FullRegexString) && DataSensor2 != CompareDS2)
            {
                registro.AgregarOActualizar(CardNumber + ID2, DataSensor2);
                CompareDS2 = DataSensor2;
                AutoSaveS2.Text = "Auto Save ✓ (Sensor 2)";
                AutoSaveS2.ForeColor = Color.Green;
                if (Default_S2())
                {
                    ConfigSensor2("Disable");
                    DefaultValues_2.Checked = true;
                    UpdateSequence.Enabled = true;
                }
            }
            else
            {
            }

            UpdateRadioButton();

        }
        private string DefaultZero(List<TextBox> textBoxes)
        {
            return string.Join(" ", textBoxes.Select(textBox => string.IsNullOrWhiteSpace(textBox.Text) ? "0" : textBox.Text));

        }
        private void Name_S1_TextChanged(object sender, EventArgs e)
        {
            Name_S1_Textchange();
        }
        private bool isAutomaticChange = false;
        private void Name_S1_Textchange()
        {
            if (isAutomaticChange)
            {
                // Restablecer el interruptor para futuras interacciones
                isAutomaticChange = false;
                return;
            }
            if (Name_S1.Text != string.Empty)
            {
                StatusS1();
            }
            else { AutoSaveS1.Text = string.Empty; }
            if (Name_S1.Text == "" && !isAutomaticChange)
            {
                ConfigSensor1("Enable");
                DefaultValues_1.Checked = false;
                registro.AgregarOActualizar(CardNumber + "S1", "");
            }
        }
        private void Name_S2_Textchange()
        {
            if (isAutomaticChange)
            {
                isAutomaticChange = false;
                return;
            }
            if (Name_S2.Text != string.Empty)
            {
                StatusS2();
            }
            else { AutoSaveS2.Text = string.Empty; }
            if (Name_S2.Text == "" && !isAutomaticChange)
            {
                ConfigSensor2("Enable");
                DefaultValues_2.Checked = false;
                registro.AgregarOActualizar(CardNumber + "S2", "");
            }
        }
        private void StatusS1()
        {
            if (Regex.IsMatch(Name_S1.Text, @"^[A-Za-z]+-\d{1,2}$"))
            {
                StatusLS1.Text = "Sensor 1: Enable";
                StatusLS1.ForeColor = Color.Green;
                DefaultValues_1.Enabled = true;
                ConfigSensor1("Enable");
                ConfirmDataS1("Enable");
            }
            else
            {
                StatusLS1.Text = "Sensor 1: Disable";
                StatusLS1.ForeColor = Color.Black;
                DefaultValues_1.Enabled = false;
                ConfigSensor1("Disable");
                ConfirmDataS1("Enable");
            }
        }

        private void Name_S2_TextChanged(object sender, EventArgs e)
        {
            Name_S2_Textchange();
        }
        private void StatusS2()
        {
            if (Regex.IsMatch(Name_S2.Text, @"^[A-Za-z]+-\d{1,2}$"))
            {
                StatusLS2.Text = "Sensor 2: Enable";
                StatusLS2.ForeColor = Color.Green;
                DefaultValues_2.Enabled = true;
                ConfigSensor2("Enable");
                ConfirmDataS2("Enable");
            }
            else
            {
                StatusLS2.Text = "Sensor 2: Disable";
                StatusLS2.ForeColor = Color.Black;
                DefaultValues_2.Enabled = false;
                ConfigSensor2("Disable");
                ConfirmDataS2("Enable");
            }

        }
        private HashSet<string> ExtractKeysFromColumn4(string filePath)
        {
            HashSet<string> keys = new HashSet<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                bool isHeader = true; // Asumimos que la primera fila es el encabezado
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!isHeader)
                    {
                        var fields = line.Split(',');
                        if (fields.Length > 5) // Asegurarse de que hay suficientes columnas
                        {
                            keys.Add(fields[4]); // Agregar la clave de la columna [4]
                        }
                    }
                    else
                    {
                        isHeader = false; // La primera línea ha sido procesada
                    }
                }
            }
            return keys;
        }
        private void EditSeq()
        {
            ViewADD = false;
            ViewClear = true;
            ViewUpdate = true;
            registro.DeleteSequence();

            ZCode_Sequence.Text = GlobalV.ExtractZcode;
            NameSequence.Text = GlobalV.ExtractNameSequence;
            Version.Text = GlobalV.ExtractVersion;

            string[] registros = GlobalV.ExtractSequences.Split('|', (char)StringSplitOptions.RemoveEmptyEntries);

            foreach (var reg in registros)
            {
                // Encuentra el primer espacio para separar el key del value
                int indexEspacio = reg.IndexOf(' ');
                if (indexEspacio != -1) // Asegura que encontramos un espacio
                {
                    // Extrae el key y elimina los espacios
                    string clave = reg.Substring(0, indexEspacio).Replace(" ", "");
                    string valor = reg.Substring(indexEspacio + 1);

                    registro.AgregarOActualizar(clave, valor);
                }
            }
            AddSequence.Visible = ViewADD;
            ClearrSequence.Visible = ViewClear;
            UpdateSequence.Visible = ViewUpdate;
            UpdateRadioButton();
        }
        private void CreateRadiobuttonTable()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;

            for (int i = 0; i < int.Parse(GlobalV.NodesUse); i++)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Text = $"Card {i + 1}";
                radioButton.AutoSize = true;
                radioButton.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                flowLayoutPanel1.Controls.Add(radioButton);
            }

        }
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked)
            {
                // Detect Auto clear //
                isAutomaticChange = true;
                DefaultValues_1.Checked = false;
                Name_S1.Text = string.Empty;
                DataSensor1("Clear");
                // Detect Auto clear //
                isAutomaticChange = true;
                DefaultValues_2.Checked = false;
                Name_S2.Text = string.Empty;
                DataSensor2("Clear");


                // ---------------------------------------------------\\
                string cardNumber = radioButton.Text.Split(' ')[1];
                Cards.Text = $"Card:  {cardNumber}";
                CardNumber = cardNumber; // Extract card num.
                selectedRadioButton = radioButton; // Save Reference.
                Sensor1State("Enable");
                Sensor2State("Enable");

                // Extrae el número de la tarjeta desde el texto del RadioButton
                int radioButtonNumber = int.Parse(radioButton.Text.Split(' ')[1]);

                // Aquí defines los patrones de búsqueda basados en el número del RadioButton.
                string patternS1 = $"{radioButtonNumber}S1";
                string patternS2 = $"{radioButtonNumber}S2";

                // Inicializa el texto para los labels.
                string S1, S2;

                // Busca en el diccionario los valores para S1 y S2.
                registro.Sequence.TryGetValue(patternS1, out S1);
                registro.Sequence.TryGetValue(patternS2, out S2);



                // Indica si el RadioButton debe marcarse como editado.
                string newText = $"Card {radioButtonNumber}";
                if (registro.Sequence.TryGetValue(patternS1, out string valueS1) && !string.IsNullOrEmpty(valueS1))
                {
                    // Si S1 tiene un valor no vacío, actualiza el texto para reflejar que S1 está editado.
                    newText += " + S1";
                }
                if (registro.Sequence.TryGetValue(patternS2, out string valueS2) && !string.IsNullOrEmpty(valueS2))
                {
                    // Si S2 tiene un valor no vacío, actualiza el texto para reflejar que S2 está editado.
                    // Esto maneja el caso de que ambos, S1 y S2, puedan estar editados.
                    newText += newText.Contains(" ") ? " & + S2" : " + S2";
                    radioButton.ForeColor = Color.Green;
                }           // Actualiza el texto del RadioButton con la nueva información.
                radioButton.Text = newText;

                // ---------------------------------------------------\\
                // Call display data
                ReadSavedSensor(S1, S2);
            }



        }
        private void ReadSavedSensor(string S1, string S2)
        {
            if (S1 != string.Empty)
            {
                string[] segments = S1.Split(' ');
                //AutoSaveS1.Text = "";
                DefaultValues_1.Enabled = true;
                TextBox[] textBoxes = new TextBox[] { Name_S1, Dwell_S1, Threshold_S1, Settle_S1, Min_S1, ThreshTime_S1, Final_S1 };

                // Iterar sobre los segmentos y asignar cada uno a un TextBox
                for (int i = 0; i < segments.Length; i++)
                {
                    // Verificar que no hay más segmentos que TextBoxes
                    if (i < textBoxes.Length)
                    {
                        textBoxes[i].Text = segments[i];
                    }
                }

                if (Default_S1())
                {
                    ConfigSensor1("Disable");
                    DefaultValues_1.Checked = true;
                }
                else
                {
                    ConfigSensor1("Enable");
                    DefaultValues_1.Checked = false;
                }
            }
            if (S2 != string.Empty)
            {
                string[] segments = S2.Split(' ');
                //AutoSaveS2.Text = "";
                DefaultValues_2.Enabled = true;
                TextBox[] textBoxes = new TextBox[] { Name_S2, Dwell_S2, Threshold_S2, Settle_S2, Min_S2, ThreshTime_S2, Final_S2 };

                // Iterar sobre los segmentos y asignar cada uno a un TextBox
                for (int i = 0; i < segments.Length; i++)
                {
                    // Verificar que no hay más segmentos que TextBoxes
                    if (i < textBoxes.Length)
                    {
                        textBoxes[i].Text = segments[i];
                    }
                }
                if (Default_S2())
                {
                    ConfigSensor2("Disable");
                    DefaultValues_2.Checked = true;
                }
                else
                {
                    ConfigSensor2("Enable");
                    DefaultValues_2.Checked = false;
                }
            }
        }
        private void UpdateRadioButton()
        {
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is RadioButton radioButton)
                {
                    // Extrae el número del RadioButton del texto.
                    int radioButtonNumber = int.Parse(radioButton.Text.Split(' ')[1].Replace(" ", ""));

                    // Define los patrones de búsqueda basados en el número del RadioButton.
                    string patternS1 = $"{radioButtonNumber}S1";
                    string patternS2 = $"{radioButtonNumber}S2";

                    // Inicializa el texto y el color por defecto.
                    string newText = $"Card {radioButtonNumber}";
                    radioButton.ForeColor = Color.Black; // Color predeterminado

                    // Verifica y actualiza según el estado de S1 y S2.
                    bool edited = false;
                    if (registro.Sequence.TryGetValue(patternS1, out string valueS1) && !string.IsNullOrEmpty(valueS1))
                    {
                        newText += " + S1";
                        edited = true;
                    }
                    if (registro.Sequence.TryGetValue(patternS2, out string valueS2) && !string.IsNullOrEmpty(valueS2))
                    {
                        newText += newText.Contains(" ") ? " & + S2" : " + S2";
                        edited = true;
                    }

                    // Actualiza el texto y, si está editado, cambia el color a verde.
                    radioButton.Text = newText;
                    if (edited)
                    {
                        radioButton.ForeColor = Color.Green;
                    }
                }
                // ClearrSequence.Visible = false;
            }
        }

        public string GetTime()
        {
            DateTime now = DateTime.Now;
            return now.ToString("hh:mm tt");
        }
        public string GetDate()
        {
            DateTime now = DateTime.Now;
            // Formatea y devuelve la fecha como un string en el formato "MMMM dd" (por ejemplo, "Abril 01")
            return now.ToString("MMMM dd");

        }
        public (string, string[]) ExtractSequence()
        {
            int GetSequences = registro.Sequence.Count(kvp => !string.IsNullOrEmpty(kvp.Value));
            string FullZcode = "ZXXXX" + GlobalV.Sequence_ZCode;
            Console.WriteLine(FullZcode);
            string[] newData =
            {
                "N/A", // ID
                "Test", // Owner
                 GetDate(), // Date
                 GetTime(), // Hour
                 FullZcode, // Z Code, la clave que no cambia
                 Version.Text, // Version Sequence
                 NameSequence.Text, // Name Sequence
                 "S1-"+
                       GlobalV.Dweel_S1+"_"+
                       GlobalV.Threshold_S1+"_"+
                       GlobalV.Settle_S1+"_"+
                       GlobalV.Min_S1+"_"+
                       GlobalV.Time_S1+"_"+
                       GlobalV.Final_S1, // Test Values
                 GetSequences.ToString(), // Num Holders
                 GetConcatenatedValues(), // Sequence
         };

            // Devuelve tanto FullZcode como newData
            return (FullZcode, newData);
        }
        private bool AddCSV()
        {
            ConcatSteps();
            string GetSequence = GetConcatenatedValues();
            if (!string.IsNullOrEmpty(GetSequence))
            {
                // Define los datos del nuevo registro
                var (zCode, NewData) = ExtractSequence();
                // Ahora, usa 'zCode' y 'datos' en tus otras operaciones.
                AddNewCSVRecord(NewData); // Asume que esta función espera esos tipos de argumentos.
                UpdateDatagridview();
                ErrorSequence.Text = "Sequence " + zCode + " as Added succesfully.";
                ErrorSequence.ForeColor = Color.Green;
                return true;
            }
            else
            {

                ErrorSequence.Text = "Error in Generate Sequence";
                ErrorSequence.ForeColor = Color.OrangeRed;
                return false;
            }

        }
        private bool UpdateCsv()
        {
            ConcatSteps();
            string GetSequence = GetConcatenatedValues();
            if (!string.IsNullOrEmpty(GetSequence))
            {
                var (zCode, datos) = ExtractSequence();
                // Ahora, usa 'zCode' y 'datos' en tus otras operaciones.
                UpdateCSVRecord(CsvPath, zCode, datos); // Asume que esta función espera esos tipos de argumentos.
                UpdateDatagridview(); // Refresca tu vista de datos, asumiendo que DataViewCSV es un componente que lo soporta.
                ErrorSequence.Text = "Sequence " + zCode + " has been edited successfully."; // Establece un mensaje de éxito con el ZCode.
                ErrorSequence.ForeColor = Color.Green;
                return true;
            }
            else
            {
                ErrorSequence.Text = "Error in Generate Sequence";
                ErrorSequence.ForeColor = Color.OrangeRed;
                return false;
            }
        }
        private void DeleteCsv()
        {
            char columnaLetra = 'E';
            string valorABuscar = "ZXXXX" + Search_ZCode.Text; // Cambia esto por el valor que deseas buscar y eliminar
            // Convertir la letra de la columna a índice (0 basado)
            int columnIndex = columnaLetra - 'A';
            // Leer todas las líneas del archivo
            var lines = File.ReadAllLines(CsvPath).ToList();
            // Filtrar las líneas
            lines = lines.Where((line, index) =>
            {
                if (index == 0) return true; // Mantener la cabecera si existe
                var cells = line.Split(',');
                // Verificar si el índice es válido para evitar errores por índice fuera de rango
                if (columnIndex >= cells.Length)
                {
                    Console.WriteLine("Índice de columna fuera de rango.");
                    return true; // Mantener la línea para evitar la eliminación debido a un error
                }
                return cells[columnIndex] != valorABuscar;
            }).ToList();
            // Escribir el contenido actualizado a un nuevo archivo o sobrescribir el existente
            File.WriteAllLines(CsvPath, lines);
            Console.WriteLine("Archivo actualizado.");
            UpdateDatagridview();
        }
       
        private void ConcatSteps()
        {
            // Aseguramos que concatenatedValues esté limpia antes de empezar
            concatenatedValues.Clear();
            foreach (var item in registro.Sequence)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    if (concatenatedValues.Length > 0)
                    {
                        concatenatedValues.Append("|");
                    }
                    else
                    {
                        concatenatedValues.Append("|");
                    }
                    concatenatedValues.Append($"{item.Key} {item.Value}");
                }
            }
        }
        public static string GetConcatenatedValues()
        {
            return concatenatedValues.ToString();
        }
        private void AddNewCSVRecord(string[] newData)
        {
            try
            {
                // Construye la nueva línea a partir de newData
                string newLine = string.Join(",", newData);
                // Añade la nueva línea al final del archivo
                File.AppendAllText(CsvPath, newLine + Environment.NewLine);
                Console.WriteLine("Nuevo registro añadido correctamente al CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir un nuevo registro al archivo CSV: {ex.Message}");
            }
        }

        private void UpdateCSVRecord(string filePath, string zCodeToUpdate, string[] newData)
        {
            try
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();
                int lineToUpdateIndex = lines.FindIndex(line => line.Contains(zCodeToUpdate));
                if (lineToUpdateIndex != -1)
                {
                    string updatedLine = string.Join(",", newData);
                    lines[lineToUpdateIndex] = updatedLine;
                    File.WriteAllLines(filePath, lines);
                    Console.WriteLine("Registro CSV actualizado correctamente.");
                }
                else
                {
                    Console.WriteLine("Z Code no encontrado en el archivo.");
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error de E/S: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el archivo CSV: {ex.Message}");
            }
        }
        private void SaveSequences_OnNewSequenceAdded()
        {
            // Habilitar tu botón aquí
            UpdateSequence.Enabled = true; // Asegúrate de reemplazar "miBoton" con el nombre real de tu botón
            Console.WriteLine("enable");
        }
    }

    public class SaveSequences
    {
        public event Action OnNewSequenceAdded; // Evento específico para la adición de nuevas secuencias
        public Dictionary<string, string> Sequence = new Dictionary<string, string>();
        public void AgregarOActualizar(string clave, string valor)
        {
            if (Sequence.ContainsKey(clave))
            {
                // Si la clave ya existe, simplemente actualizamos el valor.
                Sequence[clave] = valor;
                OnNewSequenceAdded?.Invoke();
            }
            else
            {
                // Si la clave no existe, añadimos el nuevo par clave-valor y disparamos el evento.
                Sequence.Add(clave, valor);
                OnNewSequenceAdded?.Invoke(); // Dispara el evento solo cuando se añade una nueva secuencia
            }
        }

        public void DeleteSequence()
        {
            foreach (var key in Sequence.Keys.ToList())
            {
                Sequence[key] = "";
            }

            Console.WriteLine("Todos los datos han sido eliminados.");
        }
    }
}
