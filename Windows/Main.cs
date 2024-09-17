using LeakInterface.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using LeakInterface.FileConstructor;
using LeakInterface.Global;


namespace LeakInterface
{
    public partial class Main : Form
    {
        CheckAndCreatePath fileManager = new CheckAndCreatePath();
        List<string> FilePhat = new List<string>
                                                {GlobalL.LogFiles,
                                                 GlobalL.PortConfig,
                                                 GlobalL.ScreenConfig,
                                                 GlobalL.DefaultCardConfig,
                                                 GlobalL.UserConfig,
                                                    GlobalL.L__Config,
                                                    GlobalL.L__Security,
                                                    GlobalL.L__Sequence,
                                                    GlobalL.L__Tmp,
                                                    GlobalL.L__Weetech,
                                                GlobalL.ASequence,
                                                GlobalL.DBSequence,
                                                GlobalL.Csequences,
                                                GlobalL.WeetechConfiguration};
        private System.Windows.Forms.Timer timer;
        private bool firstTime = true;
        private System.Windows.Forms.Timer delayTimer;

        public Main()
        {
            InitializeComponent();
            // Configuración inicial del temporizador de retraso
            delayTimer = new System.Windows.Forms.Timer();
            delayTimer.Interval = 3000;
            delayTimer.Tick += DelayTimer_Tick;

            // Configuración del temporizador principal
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 500; // Puedes ajustar esto según sea necesario
            timer.Tick += Timer_Tick;
            timer.Start(); // Asegúrate de iniciar el temporizador

        }
        //----- Only Write o overwrite file -----//

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (firstTime)
            {
                Constructor();
                firstTime = false;
            }
            else
            {
                timer.Stop();
                // No hagas nada, ya que el temporizador se detiene después de la primera ejecución.
            }

        }
        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            // Detener el temporizador para evitar ejecuciones múltiples
            delayTimer.Stop();

            Global.FileConstructor CTR = new Global.FileConstructor();
            CTR.DefaultConfig(GlobalL.DefaultCardConfig);
            CTR.PortConfig(GlobalL.PortConfig);
            CTR.ScreenConfig(GlobalL.ScreenConfig);
            CTR.ActiveSequence(GlobalL.ASequence);
            CTR.UserConfig(GlobalL.UserConfig);
            CTR.DBSequences(GlobalL.DBSequence);
            CTR.Weetech(GlobalL.WeetechConfiguration);
            CTR.CSequences(GlobalL.Csequences);

            // Ocultar el formulario actual
            this.Hide();

            // Crear y mostrar el nuevo formulario
            LeakControl nuevoFormulario = new LeakControl();
            nuevoFormulario.Show();

            // Actualizar el texto de label1 para indicar el estado
            label1.Text = "Status folders ok...";

            // Reiniciar el temporizador principal si es necesario
            timer.Start();
        }

        private void Constructor()
        {
            // Asumiendo que 'FilePhat' es una lista de rutas completas de archivos
            fileManager.CheckAndCreatePaths(FilePhat);

            // Iniciar el temporizador para retrasar la siguiente acción
            delayTimer.Start();
        }



        private void Main_Load(object sender, EventArgs e)
        {
            //CenterWindow();  
        }
        private void CenterWindow()
        {
            Screen[] screens = Screen.AllScreens;

            // Seleccionar una pantalla específica (en este caso, la primera pantalla)
            // Puedes cambiar el índice para seleccionar una pantalla diferente
            Screen myScreen = screens[GlobalV.SPosition];

            // Configurar la posición de la ventana principal para que se muestre en la pantalla seleccionada
            // Puedes ajustar los valores para cambiar la posición dentro de la pantalla
            Left = myScreen.Bounds.Left;
            Top = myScreen.Bounds.Top;
            int x = myScreen.Bounds.X + (myScreen.Bounds.Width - Width) / 2;
            int y = myScreen.Bounds.Y + (myScreen.Bounds.Height - Height) / 2;

            // Configurar la posición de la ventana principal para que se muestre en el centro de la pantalla seleccionada
            Location = new Point(x, y);
        }
    }
}
