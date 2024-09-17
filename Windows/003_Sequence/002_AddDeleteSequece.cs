using LeakInterface.Global;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using LeakInterface.CSVRead;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LeakInterface.Windows._003_Sequence
{
    public partial class AddDeleteEditSequence : Form
    {
        //------ Regex data compare ------\\
        Regex DisponibleHolder = new Regex(@"^[A-Z]{1}-\d{1,2}$");
        Regex CompareHolderName = new Regex(@"^[A-Z]{1}-\d{1,2}$");
        Regex Compare = new Regex(@"^\d{1,4}$");
        Regex ZReg = new Regex(@"^ZXXXX\d{4}$");
        Regex NameSeq = new Regex(@"^[a-zA-Z0-9]{3,20}$");
        Regex Format = new Regex(@"^\d{1,4}$");
        Regex SelecM = new Regex(@"^\d{1,3}$");
        Regex VerionSeq = new Regex(@"^V(\d{2})");

        //------ Dictionary to store column data with row numbers only use in Datagridview ------\\
        private Dictionary<string, string> columnDataWithRowNumbers;
     

        public void AutoLoad()
        {
            DisplaySequences();
            label13.Text = string.Empty;
            label15.Text = string.Empty;
            label16.Text = string.Empty;
            label36.Text = string.Empty;

            NameSequence.Enabled = false;
            // Sequence
            DefaultSequence.Enabled = false;
            Zero.Enabled = false;
            Charge.Enabled = false;
            Settle.Enabled = false;
            Purge.Enabled = false;
            Release.Enabled = false;
            NumHolders.Enabled = false;

            DisponibleHolders.Enabled = false;
            //SensolHolder
            DefaultSensor.Enabled = false;
            Dwell.Enabled = false;
            Threshold.Enabled = false;
            SettleSensor.Enabled = false;
            Minimun.Enabled = false;
            Threshtime.Enabled = false;
            FinalSensor.Enabled = false;
            SaveHoldConfig.Enabled = false;
            label14.Visible = false;
            // buttons 
            SaveHoldConfig.Enabled = false;


            DisponibleHolders.Enabled = false;
            //--- Need this item ---\\

            groupBox2.Visible = true;
        }
        public AddDeleteEditSequence()
        {
            InitializeComponent();
            columnDataWithRowNumbers = new Dictionary<string, string>();
        }

        private void DisplaySequences()
        {
            string filePath = GlobalL.DBSequence;
            CsvDataReader.ReadCsvFile(filePath);
            var columnNames = CsvDataReader.ColumnNames;
            var dataRows = CsvDataReader.DataRows;
            // Set up DataGridView with column names
            foreach (string columnName in columnNames)
            {
                TableAllSequences.Columns.Add(columnName, columnName);
            }
            // Populate DataGridView with data
            foreach (var dataRow in dataRows)
            {
                TableAllSequences.Rows.Add(dataRow.ToArray());
            }
            //Extract Ports \\
            int columnIndex = 1; // Index of the column you want to extract (0-based)
            string extractedValues = "";
            foreach (DataGridViewRow row in TableAllSequences.Rows)
            {
                if (!row.IsNewRow)
                {
                    extractedValues += row.Cells[columnIndex].Value + "\n";
                }
            }
            // obtain Used Ports
            GlobalVA.ExcludePortsAndUsed = extractedValues.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //---Exclude data---\\
            TableAllSequences.Columns[1].Visible = false;
            TableAllSequences.Columns[2].Visible = false;
            TableAllSequences.Columns[3].Visible = false;

            TableAllSequences.Columns[8].Visible = false;


        }

        private void ActualizarRBTN()
        {
            RadioBtnList.Controls.Clear(); // Clear data
            if (int.TryParse(NumHolders.Text, out int count))
            {
                for (int i = 0; i < count; i++)
                {
                    RadioButton rb = new RadioButton();

                    string holderKey = (i + 1).ToString();

                    if (GlobalD.resultDict.TryGetValue(holderKey, out string[] holderData))
                    {
                        if (holderData != null && holderData.Length > 0)
                        {

                            rb.Text = holderKey + " (Holder config)";
                            rb.ForeColor = Color.Green;
                        }
                        else
                        {
                            rb.Text = holderKey + " (Holder no config)";
                            rb.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        rb.Text = holderKey + " (Waiting config Holder)";
                        rb.ForeColor = Color.Gray;
                    }

                    rb.Font = new Font(rb.Font.FontFamily, 14);
                    rb.Width = 700;
                    rb.AutoSize = true;
                    rb.Location = new Point(50, 30 * i + 1);
                    rb.CheckedChanged += RadioButton_CheckedChanged;
                    RadioBtnList.Controls.Add(rb);
                    RadioBtnList.Refresh();
                }
            }
        }

        private void EditData()
        {
            if (GlobalT.SelectionColum != 0)
            {
                string csvFilePath = GlobalL.DBSequence;
                // Read the CSV file into a list of lists (representing rows and columns)
                List<List<string>> rows = File.ReadAllLines(csvFilePath)
                                               .Select(line => line.Split(',').ToList())
                                               .ToList();

                // Specify the row and column index you want to edit (zero-based indices)
                int rowIndex = GlobalT.SelectionColum; // For example, the second row
                int columnIndex = 2; // For example, the third column

                // Check if the specified row and column exist
                if (rowIndex < rows.Count && columnIndex < rows[rowIndex].Count)
                {
                    rows[rowIndex][0] = GlobalT.SelectionColum.ToString();
                    rows[rowIndex][1] = GlobalV.LogUser;
                    rows[rowIndex][2] = DateTime.Now.ToString("M");
                    rows[rowIndex][3] = DateTime.Now.ToString("t");
                    rows[rowIndex][4] = Zcode.Text;
                    rows[rowIndex][5] = NameSequence.Text;

                    if (DefaultSequence.Checked)
                    { rows[rowIndex][6] = "Default_" + Zero.Text + "_" + Charge.Text + "_" + Settle.Text + "_" + Purge.Text + "_" + Release.Text; }
                    else { rows[rowIndex][6] = "NoDefault_" + Zero.Text + "_" + Charge.Text + "_" + Settle.Text + "_" + Purge.Text + "_" + Release.Text; }

                    rows[rowIndex][7] = GlobalT.HowManyHolders;

                    // Write the modified data back to the CSV file
                    File.WriteAllLines(csvFilePath, rows.Select(row => string.Join(",", row)));

                    Console.WriteLine("Cell edited successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid row or column index.");
                }
                TableAllSequences.Rows.Clear();
                TableAllSequences.Columns.Clear();
                DisplaySequences();
                GlobalT.SelectionColum = 0;
                GlobalT.HowManyHolders = string.Empty;
            }


        }

        public void ConvertToDictionary()
        {

            foreach (var row in GlobalVA.Sequence)
            {
                if (row.Length > 0)
                {
                    string key = row[0];
                    string[] values = row.Skip(1).ToArray();
                    GlobalD.resultDict[key] = values;
                }
            }

            // Print to check
            foreach (var item in GlobalD.resultDict)
            {
                Console.WriteLine(item.Key + ": " + string.Join(", ", item.Value));
            }
        }

        private void DisplayMAxHolderUse()
        {

            for (int i = 0; i <= DisponibleHolders.Items.Count;)
            {
                i++;
                NumHolders.Items.Add(i);
            }
        }
        private void DisplayDisponiblesHolders()
        {
            if (!File.Exists(GlobalL.PortConfig))
            {
                MessageBox.Show("File does not exist.");
                return;
            }

            // Clear the ComboBox before populating it
            DisponibleHolders.Items.Clear();
            // Dictionary to store column data with row numbers.
            columnDataWithRowNumbers.Clear();


            try
            {
                using (TextFieldParser parser = new TextFieldParser(GlobalL.PortConfig))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    int currentRow = 1;

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        //--- Search NAMES Holder in column 4 ---\\
                        if (4 >= 0 && 4 < fields.Length)
                        {
                            string port = fields[1];
                            string columnData = fields[4];

                            //--- Compare length string ---\\
                            if (columnData.Length <= 5 && columnData.Length >= 1)
                            {

                                DisponibleHolders.Items.Add(columnData);
                                columnDataWithRowNumbers[columnData] = port;

                            }

                        }
                        //--- Search NAMES Holder in column 7 ---\\
                        if (7 >= 0 && 7 < fields.Length)
                        {
                            string port = fields[1];
                            string columnData = fields[7];
                            //--- Compare length string ---\\
                            if (columnData.Length <= 5 && columnData.Length >= 1)
                            {

                                DisponibleHolders.Items.Add(columnData);
                                columnDataWithRowNumbers[columnData] = port;

                            }
                        }


                        currentRow++;

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading CSV file: " + ex.Message);
            }

        }
        private void AddDeleteEditSequence_Load(object sender, EventArgs e)
        {
            AutoLoad();
            DisplayDisponiblesHolders();
            DisplayMAxHolderUse();
            foreach (var item in columnDataWithRowNumbers)
            {
                Console.WriteLine(item.Key.ToString() + " " + item.Value.ToString());
            }
        }
        private void DefaultSequence_CheckedChanged(object sender, EventArgs e)
        {
            if (DefaultSequence.Checked == true)
            {
                Zero.Text = "16";
                Zero.Enabled = false;
                Charge.Text = "32";
                Charge.Enabled = false;
                Settle.Text = "64";
                Settle.Enabled = false;
                Purge.Text = "16";
                Purge.Enabled = false;
                Release.Text = "16";
                Release.Enabled = false;
            }
            else
            {
                Zero.Text = "";
                Zero.Enabled = true;
                Charge.Text = "";
                Charge.Enabled = true;
                Settle.Text = "";
                Settle.Enabled = true;
                Purge.Text = "";
                Purge.Enabled = true;
                Release.Text = "";
                Release.Enabled = true;
            }

        }

        private void DefaultSensor_CheckedChanged(object sender, EventArgs e)
        {
            if (DefaultSensor.Checked == true)
            {
                Dwell.Text = "170";
                Dwell.Enabled = false;
                Threshold.Text = "350";
                Threshold.Enabled = false;
                SettleSensor.Text = "0";
                SettleSensor.Enabled = false;
                Minimun.Text = "280";
                Minimun.Enabled = false;
                Threshtime.Text = "0";
                Threshtime.Enabled = false;
                FinalSensor.Text = "0";
                FinalSensor.Enabled = false;
            }
            else
            {
                Dwell.Text = "";
                Dwell.Enabled = true;
                Threshold.Text = "";
                Threshold.Enabled = true;
                SettleSensor.Text = "";
                SettleSensor.Enabled = true;
                Minimun.Text = "";
                Minimun.Enabled = true;
                Threshtime.Text = "";
                Threshtime.Enabled = true;
                FinalSensor.Text = "";
                FinalSensor.Enabled = true;

            }

        }

  
        private void SaveHoldConfig_Click(object sender, EventArgs e)
        {
            EditData();
        }

    

        private void DisponibleHolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedColumnData = DisponibleHolders.SelectedItem.ToString();
            string rowNumber = columnDataWithRowNumbers[selectedColumnData];
            Console.WriteLine(selectedColumnData + " " + rowNumber);
        }
        private void TableAllSequences_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                TableAllSequences.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewRow selectedRow = TableAllSequences.SelectedRows[0];

                //--- Save colum selection ---\\
                int valFile = int.Parse(selectedRow.Cells[0].Value.ToString());
                Global.GlobalT.SelectionColum = valFile;
                //--- Z code ---\\
                string code = selectedRow.Cells[4].Value.ToString(); //Tag Z Code
                Zcode.Text = code;
                //--- Version ---\\
                string ver= selectedRow.Cells[5].Value.ToString();
                VersionSequence.Text = ver;
                //--- Name Sequence---\\
                string NameSeq = selectedRow.Cells[6].Value.ToString();//Tag Name Sequence
                NameSequence.Text = NameSeq;

                //--- Values ---\\
                string TestSequence = selectedRow.Cells[7].Value.ToString();
                string[] viewSequece = TestSequence.Split('_');
                //-- check box default values --\\
                if (viewSequece[0].ToString() == "Default")
                { DefaultSequence.Checked = true; }
                else
                {
                    Zero.Text = viewSequece[1].ToString();
                    Charge.Text = viewSequece[2].ToString();
                    Settle.Text = viewSequece[3].ToString();
                    Purge.Text = viewSequece[4].ToString();
                    Release.Text = viewSequece[5].ToString();
                    DefaultSequence.Checked = false;
                }
                string HMholder = selectedRow.Cells[8].Value.ToString();
                NumHolders.Text = HMholder;


                //--- Split string and Save in dictionary ---\\
                // Step 1: Split to # character
                string HoldSequce = selectedRow.Cells[9].Value.ToString();
                if (HoldSequce.StartsWith("#"))
                { HoldSequce = HoldSequce.Substring(1); }
                string[] row = HoldSequce.Split('#');

                // Step 2: split to _
                GlobalVA.Sequence = new string[row.Length][];
                for (int i = 0; i < row.Length; i++)
                { GlobalVA.Sequence[i] = row[i].Split('_'); }

                // Use only print data 
                for (int i = 0; i < GlobalVA.Sequence.Length; i++)
                {
                    for (int j = 0; j < GlobalVA.Sequence[i].Length; j++)
                    {
                        Console.Write(GlobalVA.Sequence[i][j] + "\t");
                    }
                    Console.WriteLine();
                }


                ConvertToDictionary();
                
                ActualizarRBTN();
                NumHolders.Refresh();

            }

            catch
            {
                Console.WriteLine("Error");

            }


        }
      
        
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    try
                    {
                        rb.Text = rb.Text.Split(' ')[0] + " (Editing ...)";
                        Console.WriteLine(rb.Text.Split(' ')[0]);
                        string key = rb.Text.Split(' ')[0];
                        string[] item = GlobalD.resultDict[key];

                        if (CompareHolderName.IsMatch(item[0]) && Compare.IsMatch(item[3]) && Compare.IsMatch(item[4]) && Compare.IsMatch(item[5]) && Compare.IsMatch(item[6]) && Compare.IsMatch(item[7]) && Compare.IsMatch(item[8]))
                        {
                            //Holder name
                            DisponibleHolders.Text = item[0];
                            //Holder Config 
                            Dwell.Text = item[3];
                            Threshold.Text = item[4];
                            SettleSensor.Text = item[5];
                            Minimun.Text = item[6];
                            Threshtime.Text = item[7];
                            FinalSensor.Text = item[8];
                        }
                    }
                    catch
                    {                            //Holder name
                        //Holder Config
                        Dwell.Text = string.Empty;
                        Threshold.Text = string.Empty;
                        SettleSensor.Text = string.Empty;
                        Minimun.Text = string.Empty;
                        Threshtime.Text = string.Empty;
                        FinalSensor.Text = string.Empty;
                        DisponibleHolders.Text = "";
                    }
                    rb.ForeColor = Color.Orange;
                    DisponibleHolders.Enabled = true;
                }
                else
                {
                    ActualizarRBTN();
                }

            }
            else { DisponibleHolders.Enabled = false; }
        }

        private void NumHolders_SelectedIndexChanged(object sender, EventArgs e)
        {

            ActualizarRBTN();
         
        }


        private void Zcode_TextChanged(object sender, EventArgs e)
        {
            //--- Only use this configuration "Z+1234567" ---\\
      
            if (ZReg.IsMatch(Zcode.Text))
            {
                //--- Clear Display ---\\ 
                label13.Text = "";
              
                label14.Visible = false;
               
                //--- Process ---\\

                Console.WriteLine("Z code it's good");

                NameSequence.Enabled = true;//Nex step (Name Values)



            }
            else
            {

                DisponibleHolders.Enabled = false;
                if (Zcode.Text.Length >= 7 || Zcode.Text.Length <= 7)
                {
                    label14.Visible = true;
                    label13.Text = "Correct format: ZXXXX5678";
       
                }
                NameSequence.Enabled = false;//Nex step (Name Values)
                //Disable options
                DefaultSequence.Enabled = false;
                Zero.Enabled = false;
                Charge.Enabled = false;
                Settle.Enabled = false;
                Purge.Enabled = false;
                Release.Enabled = false;

                //Holder num
                NumHolders.Enabled = false;
                RadioBtnList.Enabled = false;

                //Disable options 
                DisponibleHolders.Enabled = false;
                Dwell.Enabled = false;
                Threshold.Enabled = false;
                SettleSensor.Enabled = false;
                Minimun.Enabled = false;
                Threshtime.Enabled = false;
                FinalSensor.Enabled = false;
                SaveHoldConfig.Enabled = false;
                //Empty string not selection column

                if (GlobalT.SelectionColum == 0)
                {
                    Zero.Text = string.Empty;
                    Charge.Text = string.Empty;
                    Settle.Text = string.Empty;
                    Purge.Text = string.Empty;
                    Release.Text = string.Empty;
                    NameSequence.Text = string.Empty;
                    DisponibleHolders.Text = string.Empty;
                    Dwell.Text = string.Empty;
                    Threshtime.Text = string.Empty;
                    FinalSensor.Text = string.Empty;
                    Threshold.Text = string.Empty;
                    Minimun.Text = string.Empty;
                    FinalSensor.Text += string.Empty;
                    SettleSensor.Text += string.Empty;
                }



                Console.WriteLine("Zcode isn't good");
            }

        }
        private void NameSequence_TextChanged(object sender, EventArgs e)
        {
        
            if (NameSeq.IsMatch(NameSequence.Text)&&VerionSeq.IsMatch(VersionSequence.Text))
            {
              
                DefaultSequence.Enabled = true;
                Zero.Enabled = true;
                Charge.Enabled = true;
                Settle.Enabled = true;
                Purge.Enabled = true;
                Release.Enabled = true;

            }
            else
            {
                
                //Disable options
                DefaultSequence.Enabled = false;
                Zero.Enabled = false;
                Charge.Enabled = false;
                Settle.Enabled = false;
                Purge.Enabled = false;
                Release.Enabled = false;


                //Holder num
                NumHolders.Enabled = false;
                RadioBtnList.Enabled = false;

                //Disable options 
                DisponibleHolders.Enabled = false;
                Dwell.Enabled = false;
                Threshold.Enabled = false;
                SettleSensor.Enabled = false;
                Minimun.Enabled = false;
                Threshtime.Enabled = false;
                FinalSensor.Enabled = false;
                SaveHoldConfig.Enabled = false;
                //Empty string not selection column
                if (GlobalT.SelectionColum == 0)
                {
                    Zero.Text = string.Empty;
                    Charge.Text = string.Empty;
                    Settle.Text = string.Empty;
                    Purge.Text = string.Empty;
                    Release.Text = string.Empty;
                    Dwell.Text = string.Empty;
                    Threshtime.Text = string.Empty;
                    FinalSensor.Text = string.Empty;
                    Threshold.Text = string.Empty;
                    Minimun.Text = string.Empty;
                    FinalSensor.Text += string.Empty;
                    SettleSensor.Text += string.Empty;
                }


            }
        }

        //------ use to visible label error ------\\
        private void Zero_TextChanged(object sender, EventArgs e)
        {
         
            if (Format.IsMatch(Zero.Text) && Format.IsMatch(Charge.Text) && Format.IsMatch(Settle.Text) && Format.IsMatch(Purge.Text) && Format.IsMatch(Release.Text))
            {
                NumHolders.Enabled = true;//Enable next step
            }
            else
            {
                //Holder num
                NumHolders.Enabled = false;
                RadioBtnList.Enabled = false;

                //Disable options 
                DisponibleHolders.Enabled = false;
                Dwell.Enabled = false;
                Threshold.Enabled = false;
                SettleSensor.Enabled = false;
                Minimun.Enabled = false;
                Threshtime.Enabled = false;
                FinalSensor.Enabled = false;
                SaveHoldConfig.Enabled = false;
                //Empty string not selection column
                if (GlobalT.SelectionColum == 0)
                {
                    Dwell.Text = string.Empty;
                    Threshtime.Text = string.Empty;
                    FinalSensor.Text = string.Empty;
                    Threshold.Text = string.Empty;
                    Minimun.Text = string.Empty;
                    FinalSensor.Text += string.Empty;
                    SettleSensor.Text += string.Empty;
                }

            }
            if (Format.IsMatch(Zero.Text))
            { label20.Visible = false; }
            else { label20.Visible = true; }
            if (Format.IsMatch(Charge.Text))
            { label21.Visible = false; }
            else { label21.Visible = true; }
            if (Format.IsMatch(Settle.Text))
            { label22.Visible = false; }
            else { label22.Visible = true; }
            if (Format.IsMatch(Purge.Text))
            { label23.Visible = false; }
            else { label23.Visible = true; }
            if (Format.IsMatch(Release.Text))
            { label24.Visible = false; }
            else { label24.Visible = true; }


        }
        private void Charge_TextChanged(object sender, EventArgs e)
        {
            Zero_TextChanged(sender, e);
        }
        private void Settle_TextChanged(object sender, EventArgs e)
        {
            Zero_TextChanged(sender, e);
        }
        private void Purge_TextChanged(object sender, EventArgs e)
        {
            Zero_TextChanged(sender, e);
        }
        private void Release_TextChanged(object sender, EventArgs e)
        {
            Zero_TextChanged(sender, e);
        }

        private void NumHolders_TextChanged(object sender, EventArgs e)
        {


          
            if (SelecM.IsMatch(NumHolders.Text))
            {

                GlobalT.HowManyHolders = NumHolders.Text;
                RadioBtnList.Enabled = true;
                label36.Text = string.Empty;
            }
            else
            {

                label36.Text = "Use format 0-9";
                RadioBtnList.Enabled = false;
                //Disable options 
                DisponibleHolders.Enabled = false;
                Dwell.Enabled = false;
                Threshold.Enabled = false;
                SettleSensor.Enabled = false;
                Minimun.Enabled = false;
                Threshtime.Enabled = false;
                FinalSensor.Enabled = false;
                SaveHoldConfig.Enabled = false;
                //Empty string 
                if (GlobalT.SelectionColum == 0)
                {
                    Dwell.Text = string.Empty;
                    Threshtime.Text = string.Empty;
                    FinalSensor.Text = string.Empty;
                    Threshold.Text = string.Empty;
                    Minimun.Text = string.Empty;
                    FinalSensor.Text += string.Empty;
                    SettleSensor.Text += string.Empty;
                }


            }
        }
        private void DisponibleHolders_TextChanged(object sender, EventArgs e)
        {
          
            if (DisponibleHolder.IsMatch(DisponibleHolders.Text))
            {
                Dwell.Enabled = true;
                Threshold.Enabled = true;
                SettleSensor.Enabled = true;
                Minimun.Enabled = true;
                Threshtime.Enabled = true;
                FinalSensor.Enabled = true;
                DefaultSensor.Enabled = true;

            }
            else
            {

                DefaultSensor.Enabled = false;
                Dwell.Enabled = false;
                Threshold.Enabled = false;
                SettleSensor.Enabled = false;
                Minimun.Enabled = false;
                Threshtime.Enabled = false;
                FinalSensor.Enabled = false;
                SaveHoldConfig.Enabled = false;
                //clear data
                SaveHoldConfig.Enabled = false;
                if (GlobalT.SelectionColum == 0)
                {
                    Dwell.Text = string.Empty;
                    Threshtime.Text = string.Empty;
                    FinalSensor.Text = string.Empty;
                    SettleSensor.Text = string.Empty;
                }



            }
        }

        //------ End Step ------\\
        private void Dwell_TextChanged(object sender, EventArgs e)
        {
           
            if (Format.IsMatch(Dwell.Text) && Format.IsMatch(Threshold.Text) && Format.IsMatch(SettleSensor.Text) && Format.IsMatch(Minimun.Text) && Format.IsMatch(Threshtime.Text) && Format.IsMatch(FinalSensor.Text) && GlobalT.SelectionColum != 0)
            {
                SaveHoldConfig.Enabled = true;
            }
            else
            {
                SaveHoldConfig.Enabled = false;
            }

            if (Format.IsMatch(Dwell.Text))
            { label25.Visible = false; }
            else { label25.Visible = true; }
            if (Format.IsMatch(Threshold.Text))
            { label26.Visible = false; }
            else { label26.Visible = true; }
            if (Format.IsMatch(SettleSensor.Text))
            { label27.Visible = false; }
            else { label27.Visible = true; }
            if (Format.IsMatch(Minimun.Text))
            { label28.Visible = false; }
            else { label28.Visible = true; }
            if (Format.IsMatch(Threshtime.Text))
            { label29.Visible = false; }
            else { label29.Visible = true; }
            if (Format.IsMatch(FinalSensor.Text))
            { label30.Visible = false; }
            else { label30.Visible = true; }
        }
        private void Threshold_TextChanged(object sender, EventArgs e)
        {
            Dwell_TextChanged(sender, e);
        }
        private void SettleSensor_TextChanged(object sender, EventArgs e)
        {
            Dwell_TextChanged(sender, e);
        }
        private void Minimun_TextChanged(object sender, EventArgs e)
        {
            Dwell_TextChanged(sender, e);
        }
        private void Threshtime_TextChanged(object sender, EventArgs e)
        {
            Dwell_TextChanged(sender, e);
        }
        private void FinalSensor_TextChanged(object sender, EventArgs e)
        {
            Dwell_TextChanged(sender, e);
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            ConvertToDictionary();
        }

        private void VersionSequence_TextChanged(object sender, EventArgs e)
        {
            if (VerionSeq.IsMatch(VersionSequence.Text)) 
            {
                Versionl.Text="";
                label41.Visible=false;
            }
            else { label41.Visible = true; 
                Versionl.Text ="Example: V2.2 | V99";
            }
        }
    }
}
