using CustomControls.RJControls;
using LeakInterface.Global;
using LeakInterface.Windows._001_Home;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace LeakInterface
{
    public partial class FullAcces : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        //--- Mouse Windows ---\
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); }

        //--- Open Form in Form
        private void OpenForm(object OpenNewForm)
        {
            if (this.MainPanel.Controls.Count > 0)
                this.MainPanel.Controls.RemoveAt(0);
            Form OpenForm = OpenNewForm as Form;
            OpenForm.TopLevel = false;
            OpenForm.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(OpenForm);
            this.MainPanel.Tag = OpenForm;
            OpenForm.Show();
        }
        //--- Main Prog ---\\
        public FullAcces()
        {
           
            InitializeComponent();
        }
        //--- Auto Load Configuration ---\\
        private void AutoLoad()
        {
            //--- Button Admin ---\\
            //Admin.Visible = false; 
            //-- Dropdowmenu Config --\\
            DropdownMenu_Home.IsMainMenu = true;
            DropdownMenu_Home.PrimaryColor = Color.OrangeRed;
            DropdownMenu_Home.ForeColor = Color.White;

            DropdownMenu_Node.IsMainMenu = true;
            DropdownMenu_Node.PrimaryColor = Color.OrangeRed;
            DropdownMenu_Node.ForeColor = Color.White;

            DropdownMenu_Sequence.IsMainMenu = true;
            DropdownMenu_Sequence.PrimaryColor = Color.OrangeRed;
            DropdownMenu_Sequence.ForeColor = Color.White;

            DropdownMenu_Test.IsMainMenu = true;
            DropdownMenu_Test.PrimaryColor = Color.OrangeRed;
            DropdownMenu_Test.ForeColor = Color.White;

            DropdownMenu_Report.IsMainMenu = true;
            DropdownMenu_Report.PrimaryColor = Color.OrangeRed;
            DropdownMenu_Report.ForeColor = Color.White;

            labelV.Text = GlobalV.Version.ToString();

            if (GlobalT.UserLevel == "UserLevelStatus_1_ReadMode")
            {
                /*  LEVEL 1 (only read mode)
                 *  Full access Window
                 *  └> Home                              |v
                 *  |    └> Main                         |V 
                 *  |    └> Weetech Configuration        |F
                 *  └> Node                              |V
                 *  |    └> Nodes ready to use           |V
                 *  |    └> Add, Edit, Delete Nodes      |F
                 *  └> Sequence                          |V
                 *  |    └> Active sequence              |V
                 *  |    └> Compare and change sequence  |F
                 *  |    └> Add, Edit, Delete sequence   |F
                 *  └> Test                              |F
                 *  |    └> Display All Results          |F
                 *  |    └> Individual Test              |F
                 *  |    └> Multi Test                   |F
                 *  └> Report     
                 */

                //---- Buttons ----\\
                Admin.Visible = false;//Disable Admin mode
                Test.Visible = false; //Disable Test mode

                //---- Submenus ----\\
                toolStripMenuItem3.Visible = false;//Weetech configuration
                addEditDeleteAndConfigureNodesToolStripMenuItem.Visible = false; //Add, edit and delte nodes
                addEditDeleteSequenceToolStripMenuItem.Visible = false;//Change sequence 
                toolStripMenuItem1.Visible = false; //Add, edit, delete sequence
            }
            else if (GlobalT.UserLevel == "UserLevelStatus_2_ReadAndWriteMode")
            {/*  LEVEL 1 (only read mode)
                 *  Full access Window
                 *  └> Home                              |v
                 *  |    └> Main                         |V 
                 *  |    └> Weetech Configuration        |V
                 *  └> Node                              |V
                 *  |    └> Nodes ready to use           |V
                 *  |    └> Add, Edit, Delete Nodes      |V
                 *  └> Sequence                          |V
                 *  |    └> Active sequence              |V
                 *  |    └> Compare and change sequence  |V
                 *  |    └> Add, Edit, Delete sequence   |V
                 *  └> Test                              |F
                 *  |    └> Display All Results          |F
                 *  |    └> Individual Test              |F
                 *  |    └> Multi Test                   |F
                 *  └> Report     
                 */

                //---- Buttons ----\\
                Admin.Visible = false;//Disable Admin mode
                Test.Visible = false; //Disable Test mode
                //---- Submenus ----\\
                //Enable All
            }
            else if (GlobalT.UserLevel == "UserLevelStatus_3_FullAccessMode")
            {
                Admin.Visible = false;//Disable Admin mode
             }
            else if (GlobalT.UserLevel == "UserLevelStatus_develop_Key1004098ws")
            { }
            else
            { //this.Close();
            }
        }
        //--- Level Users ---\\
        private void LevelUsers()
        {
        }
        //--- Main Load data ---\\
        private void FullAcces_Load(object sender, EventArgs e)
        {
            
            
            AutoLoad();
            LevelUsers();
        }
        //--- Retur btn ---\\
        private void rjButton1_Click(object sender, EventArgs e)
        {
            GC.Collect();//---Clear Data---\\
            this.Close();
        }

        //--- Maximized Window ---\\
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            pictureBox4.Visible = true;
            Maxi.Visible = false;
        }
        //--- Normalized Window ---\\
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            pictureBox4.Visible = false;
            Maxi.Visible = true;
        }
        //--------- btn Select ---------\\
        private void rjButton9_Click(object sender, EventArgs e)
        { DropdownMenu_Home.Show(rjButton9, rjButton9.Width, 0); }
        private void rjButton10_Click(object sender, EventArgs e)
        { DropdownMenu_Node.Show(rjButton10, rjButton10.Width, 0); }
        private void rjButton11_Click(object sender, EventArgs e)
        { DropdownMenu_Sequence.Show(rjButton11, rjButton11.Width, 0); }
        private void Test_Click(object sender, EventArgs e)
        { DropdownMenu_Test.Show(Test, Test.Width, 0); }
        private void Admin_Click(object sender, EventArgs e)
        { OpenForm(new Windows._005_Security._000_Admin()); label50.Text = "Admin"; }
        //--------
        private void nodesReadyToUseToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._002_Node.DisplayAllNodes()); label50.Text = "Nodes Ready to use"; }
        private void addEditDeleteAndConfigureNodesToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._002_Node._002_LeakPort()); label50.Text = "Add, Edit, Delete and configure Nodes"; }
        private void mainSecuenceToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._001_Home.DisplayInfo()); label50.Text = "Main Sequence"; }
        private void activeSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._003_Sequence.ActiveSeq()); label50.Text = "Active sequence"; }
        private void addEditDeleteSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._001_Home.ChangeSequence()); label50.Text = "Change Sequence"; }


        private void weToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._004_Test.DisplayAllResults()); label50.Text = "Display all Result"; }
        private void weqToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._004_Test.individualTest()); label50.Text = "Individual Test"; }
        private void multiTestToolStripMenuItem_Click(object sender, EventArgs e)
        { OpenForm(new Windows._004_Test.MultiTest()); label50.Text = "Multi test"; }
        private void pictureBox1_Click(object sender, EventArgs e)
        { GC.Collect(); Application.Restart(); }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenForm(new Windows._003_Sequence._002_AddEditDeleteSequences()); ; label50.Text = "Add, Edit and Delete Sequence";
            this.WindowState = FormWindowState.Maximized;
            pictureBox4.Visible = true;
            Maxi.Visible = false;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenForm(new WeetechConfig()); ; label50.Text = "Weetech Configuration";
            pictureBox4.Visible = true;
            Maxi.Visible = false;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void Report_Click(object sender, EventArgs e)
        {
            DropdownMenu_Report.Show(Test, Test.Width, 0);

        }

        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            OpenForm(new Windows._005_Report.DBReport()); label50.Text = "View Report DB";
        }
    }
}
