using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using LeakInterface.Global;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using LeakInterface.Windows;
using System.IO;
using System.Threading;
using LeakInterface.Graphic;
using CsvHelper;
using System.Windows.Markup;

namespace LeakInterface
{

    public partial class PopState : Form

    {
        Regex Zcode = new Regex(@"^Z\d{8}$");
        Regex state = new Regex(@"^(Test|Wait|Start)$");
        Regex Pass = new Regex(@"^(True|False)$");
        Regex Error = new Regex(@"^[A-Za-z0-9]+\.$");
  
        public event Action<string> MessageReceived;


        public PopState()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetRoundShape();

            //--- Windows start position ---\\
            /*this.StartPosition = FormStartPosition.Manual;
            int centerX = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            int topY = 0;
            this.Location = new Point(centerX, topY);
            this.FormBorderStyle = FormBorderStyle.None;
            */
        }
        //--- Window radius ---\\\
        private void SetRoundShape()
        {
            this.Region = null;

            GraphicsPath formPath = new GraphicsPath();
            Rectangle formRectangle = this.ClientRectangle;

            int cornerRadius = 30; // Windows radius variable


            formPath.StartFigure();
            formPath.AddLine(formRectangle.X, formRectangle.Y, formRectangle.Width, formRectangle.Y);
            formPath.AddLine(formRectangle.Width, formRectangle.Y, formRectangle.Width, formRectangle.Height - cornerRadius);
            formPath.AddArc(formRectangle.X + formRectangle.Width - cornerRadius, formRectangle.Y + formRectangle.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            formPath.AddArc(formRectangle.X, formRectangle.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90);

            formPath.CloseFigure();

            using (Graphics graphics = this.CreateGraphics())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                this.Region = new Region(formPath);
            }
        }



        //---------XML Write ----------//

        private void State_DoubleClick(object sender, EventArgs e)
        {
            GC.Collect();
            this.Close();
        }

        private void State_Load(object sender, EventArgs e)
        {
            //CenterWindow();
            TopMost = true;


            ReadState();

            GC.Collect();


            /*
           BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.DoWork += BgWorker_DoWork;

            bgWorker.RunWorkerAsync();
            */
        }
        private void CenterWindow()
        {
            // Obtener todas las pantallas conectadas
            Screen[] screens = Screen.AllScreens;

            // Seleccionar una pantalla específica (en este caso, la primera pantalla)
            // Puedes cambiar el índice para seleccionar una pantalla diferente
            Screen myScreen = screens[GlobalV.SPosition];


            // Calcular la posición central horizontal para la ventana
            int x = myScreen.Bounds.X + (myScreen.Bounds.Width - this.Width) / 2;
            // Configurar la posición 'y' cerca del borde superior de la pantalla
            int y = myScreen.Bounds.Y + 50; // Puedes ajustar este valor según tus preferencias

            // Configurar la posición de la ventana principal para que se muestre en la parte superior y centrada horizontalmente
            this.Location = new Point(x, y);


        }
        private void ReadState()
        {
           
            if (GlobalV.StateLeak == 1)
            {
                //--- Disble Auto Leak ---\\
                OnOffbtn.Checked = true;
                LeakIsDisable.Visible = false ;
            }
            else
            {
                //--- Enable Auto Leak ---\\
                OnOffbtn.Checked = false;
                LeakIsDisable.Visible = true;
                
              
             }
        }
    }
}
