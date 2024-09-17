using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using IniParser;
using LeakInterface.Global;
using Microsoft.Win32;

namespace LeakInterface.Windows._005_Security
{
    public partial class _000_Admin : Form
    {
        public static string PCUser = Environment.UserName;
        string grouptext;
        string Filter, Title, DefaultExt, FileName;
        string InPath;

        public _000_Admin()
        {
            InitializeComponent();
        }

        private void _000_Admin_Load(object sender, EventArgs e)
        {
            PhatFile.Text = string.Empty;
            OkandErrorlbl.Text = string.Empty;
            viewerGroupbox.Text = "";
            SaveBtn.Visible = false;
            SaveFile.Visible = false;
        }

        //---- Display Elements ----\\
        private void ViewSave()
        {
            SaveBtn.Visible = true;
            SaveFile.Visible = true;
        }
        private void NoViewSave()
        {
            SaveBtn.Visible = false;
            SaveFile.Visible = false;
        }

        //---- View Buttons ----\\
        private void view()
        {
            try
            {
                // Read all text from the specified file
                string fileContent = File.ReadAllText(InPath);
                PhatFile.Text = $"Location : " + InPath;
                // Set the text of textBox1
                DisplayData.Text = fileContent;
                DisplayData.ReadOnly = true;
                viewerGroupbox.Text = grouptext;
                OkandErrorlbl.Text = string.Empty;
                NoViewSave();
            }
            catch (Exception ex)
            {
                PhatFile.Text = ("An error occurred: " + ex.Message);
            }
        }
        private void WeetechView_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.ASequence);
            grouptext = "Read Only (Weetech file)";
            view();
        }
        private void UsersView_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.UserConfig);
            grouptext = "Read Only (Users Admin)";
            view();
        }
        private void ASecuenceView_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.ASequence);
            grouptext = "Read Only (Active Sequence)";
            view();
        }
        private void PortsView_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.PortConfig);
            grouptext = "Read Only (Ports)";
            view();
        }
        private void SequenceView_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.DBSequence);
            grouptext = "Read Only (Sequences)";
            view();
        }


        //---- Save File ----\\
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string content = DisplayData.Text;
                string path = InPath;
                OkandErrorlbl.Text = "Update complete.";
                OkandErrorlbl.ForeColor = Color.Green;
                File.WriteAllText(path, content);
            }
            catch (Exception ex)
            {
                OkandErrorlbl.Text = ex.ToString();
                OkandErrorlbl.ForeColor = Color.Red;
                Console.WriteLine("Data error");
            }

        }
        //---- Edit Button File ----\\
        private void save()
        {
            try
            {
                // Read all text from the specified file
                string fileContent = File.ReadAllText(InPath);
                PhatFile.Text = $"Location : " + InPath;
                // Set the text of textBox1
                DisplayData.Text = fileContent;
                DisplayData.ReadOnly = false;
                viewerGroupbox.Text = grouptext;
                ViewSave();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void WeetechEdit_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.ASequence;
            grouptext = "Editing (Weetech file)";
            save();
        }
        private void UsersEdit_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.UserConfig;
            grouptext = "Editing (Users Admin)";
            save();

        }
        private void ASequenceEdit_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.ASequence;
            grouptext = "Editing (Active Sequence)";
            save();
        }
        private void PortsEdit_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.PortConfig;
            grouptext = "Editing (Ports)";
            save();
        }
        private void SequenceEdit_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.DBSequence;
            grouptext = "Editing (Sequences)";
            save();
        }


        //---- Dowload Files ----\\
        private void SaveFileIn()
        {
            try
            {
                using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog())
                {
                    saveFileDialog.Filter = Filter; // Example filter for .txt files
                    saveFileDialog.Title = Title;
                    saveFileDialog.DefaultExt = DefaultExt; // Default extension
                    saveFileDialog.FileName = FileName; // Default file name
                    saveFileDialog.InitialDirectory = @"C:\Users\" + PCUser + "\\Desktop"; // Default directory

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string path = saveFileDialog.FileName;

                        // Now you can save your data to the selected path
                        File.WriteAllText(path, DisplayData.Text);
                    }
                }
                DisplayData.Text = string.Empty;
                OkandErrorlbl.Text = "Dowload Complete";
                OkandErrorlbl.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                OkandErrorlbl.Text = ex.Message;
                OkandErrorlbl.ForeColor = Color.Red;
            }
        }
        private void WeetechDownload_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.ASequence);
            view();
            DefaultExt = ".ini";
            FileName = "WeetechConfig";
            Filter = "My Specific Files|*" + DefaultExt; // Example filter for .txt files
            Title = "Download Weetech File";
            SaveFileIn();
        }
        private void UsersDownload_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.UserConfig);
            view();
            DefaultExt = ".xml";
            FileName = "UsersConfig";
            Filter = "My Specific Files|*" + DefaultExt; // Example filter for .txt files
            Title = "Download Users File";
            SaveFileIn();
        }
        private void ASequenceDownload_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.ASequence);
            view();
            DefaultExt = ".ini";
            FileName = "ActiveSequence";
            Filter = "My Specific Files|*" + DefaultExt; // Example filter for .txt files
            Title = "Download Actice Sequence File";
            SaveFileIn();
        }
        private void PortsDownload_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.PortConfig);
            view();
            DefaultExt = ".csv";
            FileName = "PortConfig";
            Filter = "My Specific Files|*" + DefaultExt; // Example filter for .txt files
            Title = "Download Port Config File";
            SaveFileIn();
        }
        private void SequenceDownload_Click(object sender, EventArgs e)
        {
            InPath = (GlobalL.DBSequence);
            view();
            DefaultExt = ".csv";
            FileName = "DB";
            Filter = "My Specific Files|*" + DefaultExt; // Example filter for .txt files
            Title = "Download Data Base File";
            SaveFileIn();
        }

        //---- Upload Files ----\\
        public bool FileExists(string filePath)
        {
            return File.Exists(InPath);
        }
        private void UploadFile()
        {

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Specific Files (" + FileName+DefaultExt+")|"+FileName + DefaultExt;
            openFileDialog.Title = "Select "+FileName+DefaultExt;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {


                // Verify that the chosen file has the correct name and extension.
              
                string selectedFilePath = openFileDialog.FileName;
                Console.WriteLine(selectedFilePath);
                // Check using GetFileName
                bool isCorrectFileName = System.IO.Path.GetFileName(selectedFilePath).Equals(FileName + DefaultExt, StringComparison.OrdinalIgnoreCase);
                // Alternatively, check using GetExtension
                bool isCorrectExtension = System.IO.Path.GetExtension(selectedFilePath).Equals(DefaultExt, StringComparison.OrdinalIgnoreCase);


                if (isCorrectFileName && isCorrectExtension)
                {
                    try
                    {
                        string destinationPath = (InPath);
                        if (FileExists(InPath))
                        {
                            File.Copy(selectedFilePath, destinationPath, true);
                            Console.WriteLine("Replace Complete");

                        }
                        else
                        {
                            Console.WriteLine("Create complete ");
                            // Ensure the directory exists or create it.
                            DirectoryInfo directoryInfo = Directory.CreateDirectory(destinationPath);

                            // Copy the file to the specific location with the specific name and extension
                            File.Copy(selectedFilePath, destinationPath, true);
                        }
                        PhatFile.Text = "File("+ FileName + DefaultExt + ")Replace Complete";
                        

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please select the correct file (" + FileName + DefaultExt + ").");
                    }
                }
                else 
                {
                    // Notify the user about the incorrect file
                    MessageBox.Show($"Please select the correct file ({FileName + DefaultExt}).");
                }
               

            }
        }
        private void WeetechUpload_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.ASequence  ;
            FileName = "WeetechConfig";
            DefaultExt = ".ini";

            UploadFile();
        }
        private void UserUpload_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.UserConfig;
            FileName = "UsersConfig";
            DefaultExt = ".xml";
            
            UploadFile();
        }
        private void ActiveSequenceUpload_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.ASequence;
            FileName = "ActiveSequence";
            DefaultExt = ".ini";
            
            UploadFile();
        }
        private void PortsUpload_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.PortConfig;
            FileName = "PortConfig";
            DefaultExt = ".csv";

            UploadFile();
        }
        private void SequenceUpload_Click(object sender, EventArgs e)
        {
            InPath = GlobalL.DBSequence;
            FileName = "DB";
            DefaultExt = ".csv";

            UploadFile();
        }



        //---- History Files ----\\
        private void DisplayLogFile( string Paht)
        {
            if (File.Exists(Paht)) 
            {
                string LogData =  File.ReadAllText(Paht);
                DisplayData.Text = LogData;
            }
        }
        private void WeetechHistory_Click(object sender, EventArgs e)
        {
            DisplayLogFile(GlobalL.L__Weetech);
        }
        private void UserHistory_Click(object sender, EventArgs e)
        {
            DisplayLogFile(GlobalL.L__Security);
            // and I need users ADD, Delete, and edit
        }
        private void ActiveSequenceHistory_Click(object sender, EventArgs e)
        {
            //HistoryFile();
        }
        private void PortsHistory_Click(object sender, EventArgs e)
        {
            DisplayLogFile(GlobalL.L__Config);
        }
        private void SequenceHistory_Click(object sender, EventArgs e)
        {
            //HistoryFile();
        }

    }
}
