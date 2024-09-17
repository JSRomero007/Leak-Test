using IniParser.Model;
using IniParser;
using LeakInterface.Global;
using LeakInterface.Graphic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace LeakInterface.Windows._006_Graphic
{
    public partial class _001_MainPlotter : Form
    {

        public _001_MainPlotter()
        {
            InitializeComponent();
            Zcode.Text = GlobalT.T_Zcode;
            labelV.Text = GlobalV.Version;
            this.WindowState = FormWindowState.Maximized;
        }
        private void OpenForm(object OpenNewForm)
        {
            if (this.MainPanel.Controls.Count == 0)
            {
                Form OpenForm = OpenNewForm as Form;

                OpenForm.TopLevel = false;
                OpenForm.Dock = DockStyle.Fill;

                // Suscribir al evento FormClosed
                OpenForm.FormClosed += (sender, e) =>
                {
                    // Cerrar el formulario principal cuando se cierre el formulario dentro del panel
                    this.Close();
                };
                this.MainPanel.Controls.Add(OpenForm);
                this.MainPanel.Tag = OpenForm;
                OpenForm.Show();
            }
        }

        public void Process()
        {
            TopMost = true;
            OpenForm(new Plot(this));
            Console.WriteLine();
        }

        private void _001_MainPlotter_Load(object sender, EventArgs e)
        {
            Process();
        }
    }
}
