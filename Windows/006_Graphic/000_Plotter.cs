
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows.Forms;
using LeakInterface.Global;
using System.Windows.Media;
using System.IO.Ports;
using System.Threading;
using IniParser.Model;
using IniParser;
using System.Linq;
using System.Collections;
using System.Windows.Markup;


namespace LeakInterface.Graphic
{
    public partial class Plot : Form
    {
        private Windows._006_Graphic._001_MainPlotter DisplayPlotter;
        static SerialPort serialPort;
        static byte[] ringbuffer = new byte[1024];
        static int headptr = 0, insertptr = 0;
        static int ringsize = ringbuffer.Length;
        static bool continueReading = true;
        static int timeSinceLastRead = 0;
        static int readTimeout = 1000;
        static int CurrentCard = 0;
        static List<string> fullString = new List<string>();
        static int dta = 0;
        private Thread requestThread;
        private Thread readThread;

        List<int> ListData = new List<int>();
        private void ResetVariables()
        {
            headptr = 0;
            insertptr = 0;
            continueReading = true;
            timeSinceLastRead = 0;
            CurrentCard = 0;
            fullString.Clear();
            dta = 0;
        }


        public void Start()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(GlobalL. ASequence);
            data["Config"]["ZCode"] = GlobalT.T_Zcode;
            data["Config"]["State"] = "Testing";
            data["Config"]["Pass"] = "True";
            data["Config"]["Error"] = "";
            parser.WriteFile(GlobalL.ASequence, data);
        }
        public void Preload()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public Plot(Windows._006_Graphic._001_MainPlotter principalForm)
        {
            InitializeComponent();
            this.DisplayPlotter = principalForm;
            ResetVariables();
            InicializarGrafico();
            Preload();
            Task.Run(() => Main());
            this.FormClosing += Plot_FormClosing;
        }
        private void InicializarGrafico()
        {
            var serie = new LineSeries
            {
                Title = "PSI",
                Values = new ChartValues<double>(), // Inicialmente vacío
                StrokeThickness = 4,
                StrokeDashArray = new System.Windows.Media.DoubleCollection(50),
                Stroke = new SolidColorBrush(Color.FromRgb(GlobalC.R[4], GlobalC.G[4], GlobalC.B[4])),
                Fill = Brushes.Transparent,
                PointGeometry = null,
                LineSmoothness = 2,
            };
            cartesianChart1.Series.Add(serie);

            if (ListData.Count != 0)
            {
                Console.WriteLine(ListData.Max());
            }
            else { Console.WriteLine("data not found"); }
            LimitLines();
            cartesianChart1.AxisX.Add(new Axis
            {
                Sections = new SectionsCollection
    {
        new AxisSection
        {
            Value = 220, // Primer límite, puedes ajustar este valor según tus necesidades
            SectionWidth = 0.1, // Ancho de la sección (puedes ajustar según tus necesidades)
            Stroke = Brushes.Red,
            StrokeThickness = 1,

        },
        new AxisSection
        {
            Value = 450, // Segundo límite, ajusta este valor según tus necesidades (debería ser diferente al primer valor)
            SectionWidth = 0.1, // Ancho de la sección (puedes ajustar según tus necesidades)
            Stroke = Brushes.Red,
            StrokeThickness = 1,

        }
    }
            });
            ((ChartValues<double>)cartesianChart1.Series[0].Values).Clear(); // Limpiar valores existentes
            cartesianChart1.Series.Add(serie);
            ((ChartValues<double>)cartesianChart1.Series[0].Values).Add(dta);
        }
        public void LimitLines()
        {
              // Static lines 
            cartesianChart1.AxisY.Add(new Axis
            { 
                        Sections = new SectionsCollection
                        {
                    //THIS Line its the limit Max pressure  ↑↑ 
                        new AxisSection
                        {
                         Value = 245, // Primer límite, puedes ajustar este valor según tus necesidades
                         SectionWidth = .1, // Ancho de la sección (puedes ajustar según tus necesidades)
                         Stroke = Brushes.Orange,
                         StrokeThickness = 1,
                        },
                        new AxisSection
                        {            
                    //THIS Line is the limit 2% in this case we need more that one  
                    //-- i need conditionate this line but i need more tha one 
                    //{
                    //}
                          Value = 240, // Primer límite, puedes ajustar este valor según tus necesidades
                          SectionWidth = .1, // Ancho de la sección (puedes ajustar según tus necesidades)
                          Stroke = Brushes.Gray,
                          StrokeThickness = 1,
                        },
                    //This line its the limit Min pressure ↓↓
                        new AxisSection
                        {
                          Value = 235, // Primer límite, puedes ajustar este valor según tus necesidades
                          SectionWidth = .1, // Ancho de la sección (puedes ajustar según tus necesidades)
                          Stroke = Brushes.Orange,
                          StrokeThickness = 1,
                        }
                    }
            });
        }
        public void READreciVing(int Data)
        {
            ListData.Add(Data);
            if (cartesianChart1.Series.Count == 0)
            {
                InicializarGrafico(); // Asegurarte de que el gráfico esté inicializado si no hay series
            }
            if (Data > 40)
            {
                label1.Text = Data.ToString();
                ((ChartValues<double>)cartesianChart1.Series[0].Values).Add(Data);
            }
            else
            {
                ((ChartValues<double>)cartesianChart1.Series[0].Values).Add(0);
            }
            //((ChartValues<double>)cartesianChart1.Series[1].Values)[0] = Data;
        }

        private void Plot_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((ChartValues<double>)cartesianChart1.Series[0].Values).Clear();
            serialPort?.Close();
        }
        public void Main()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
            ((ChartValues<double>)cartesianChart1.Series[0].Values).Clear();
            ResetVariables();
            requestThread?.Join();
            readThread?.Join();
            requestThread = new Thread(StartRequests);
            readThread = new Thread(ReadSerialPort);
            string puerto = "COM8";
            int baudrate = 38400;
            int EndTest = 0;
            serialPort = new SerialPort(puerto, baudrate);

            try
            {
                EndTest = 500;
                serialPort.Open();
                const UInt32 LEAK_ZEROTIME = 28, LEAK_CHARGETIME = 32;  // all times in 1/16 seconds
                const UInt32 LEAK_SETTLETIME = 32, LEAK_DWELLTIME1 = 240;
                const UInt32 LEAK_DWELLTIME2 = 240, LEAK_PURGETIME = 16;
                const UInt32 LEAK_THRESHOLD1 = 300, LEAK_THRESHOLD2 = 300;
                const UInt32 LEAK_RELINAIR = 16;
                ushort cksum = 0;
                byte[] buffer = new byte[264];

                int ii = 0;
                buffer[ii++] = 0xe5;  // 0
                buffer[ii++] = 0x29;  // 1
                buffer[ii++] = 0x2;  // 2
                buffer[ii++] = 0x0;  // 3
                buffer[ii++] = 0x16;
                buffer[ii++] = 0xff;  //CarID = all
                buffer[ii++] = (byte)LEAK_ZEROTIME & 0xff;
                buffer[ii++] = (byte)(LEAK_ZEROTIME >> 8) & 0xff;
                buffer[ii++] = (byte)LEAK_CHARGETIME & 0xff;
                buffer[ii++] = (byte)(LEAK_CHARGETIME >> 8) & 0xff;
                buffer[ii++] = (byte)LEAK_SETTLETIME & 0xff;
                buffer[ii++] = (byte)(LEAK_SETTLETIME >> 8) & 0xff;
                buffer[ii++] = (byte)(LEAK_PURGETIME & 0xff);
                buffer[ii++] = (byte)(LEAK_PURGETIME >> 8) & 0xff;
                buffer[ii++] = (byte)LEAK_RELINAIR & 0xff;
                buffer[ii++] = (byte)(LEAK_RELINAIR >> 8) & 0xff;
                buffer[ii++] = (byte)00; //cardid
                buffer[ii++] = (byte)(LEAK_DWELLTIME1 & 0xff);
                buffer[ii++] = (byte)((LEAK_DWELLTIME1 >> 8) & 0xff);
                buffer[ii++] = (byte)(LEAK_THRESHOLD1 & 0xff);
                buffer[ii++] = (byte)(LEAK_THRESHOLD1 >> 8) & 0xff;
                buffer[ii++] = (byte)(LEAK_DWELLTIME2 & 0xff);
                buffer[ii++] = (byte)((LEAK_DWELLTIME2 >> 8) & 0xff);
                buffer[ii++] = (byte)(LEAK_THRESHOLD2 & 0xff);
                buffer[ii++] = (byte)(LEAK_THRESHOLD2 >> 8) & 0xff;

                buffer[2] = (byte)((ii - 6) & 0xff);
                buffer[3] = (byte)(((ii - 6) >> 8) & 0xff);
                for (int i = 0; i < ii; i++)
                {cksum += buffer[i]; //outp[ii++] = buffer[i];
                }
                buffer[ii++] = (byte)((~cksum + 1) & 0xff);
                buffer[ii++] = (byte)(((~cksum + 1) >> 8) & 0xff);
                serialPort.Write(buffer, 0, ii);

                requestThread.Start();
                readThread.Start();
                // this data || its proportional 20 Series = 1 seg 
                while (continueReading && fullString.Count <= EndTest)
                {
                    if (fullString.Count > 0)
                    {
                        // Actualizar la interfaz de usuario y la gráfica con el nuevo dato
                        int DV = Int32.Parse(fullString[0]);
                        this.Invoke((MethodInvoker)delegate
                        { READreciVing(DV);});
                        // Eliminar el dato procesado de la lista
                        fullString.RemoveAt(0);
                    }
                    if (fullString.Count <= EndTest)
                    {
                        Thread.Sleep(30); // Ajusta este tiempo según sea necesario
                    }
                }
                continueReading = false;
                this.Invoke((MethodInvoker)delegate
                {
                    Thread.Sleep(3000);
                    // Cerrar el formulario actual y mostrar el formulario principal
                    this.Close(); this.FormClosing += (s, e) =>
                    {
                        continueReading = false;
                        if (requestThread != null && requestThread.IsAlive)
                            requestThread.Join();
                        if (readThread != null && readThread.IsAlive)
                            readThread.Join();
                    };
                    //InTest();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al abrir el puerto serial: " + ex.Message);
            }
        }
        static void ReadSerialPort()
        {
            while (continueReading)
            {
                try
                {
                    int bytesToRead = serialPort.BytesToRead;
                    if (bytesToRead > 0)
                    {
                        byte[] buffer = new byte[bytesToRead];
                        serialPort.Read(buffer, 0, bytesToRead);
                        ProcessBuffer(buffer);
                        timeSinceLastRead = 0; // Reiniciar el contador de tiempo
                    }
                    else
                    {
                        if (timeSinceLastRead >= readTimeout)
                        {
                            Console.WriteLine("Tiempo de espera superado, no se recibieron más datos.");
                            break; // Salir del bucle si se supera el tiempo de espera
                        }
                        Thread.Sleep(100); // Espera un poco antes de la siguiente verificación
                        timeSinceLastRead += 100;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer del puerto serial: " + ex.Message);
                    break;
                }
            }
        }
        static void ProcessBuffer(byte[] buffer)
        {
            foreach (byte b in buffer)
            {
                ringbuffer[insertptr] = b;
                insertptr = (insertptr + 1) % ringsize;
                if (b == 0xE5)
                {
                    if (headptr != insertptr)
                    {
                        int ret = Unpacket(ringbuffer, headptr, insertptr);
                        headptr = (headptr + ret) % ringsize;
                    }
                }
            }
        }
        static int Unpacket(byte[] buffer, int start, int end)
        {
            int i = start;
            int count = 0; // Contador para los pares de valores
            bool startCounting = false; // Flag para comenzar a contar desde '28'
            StringBuilder sequenceBuilder = new StringBuilder(); // Para construir la secuencia de salida
            List<int> values = new List<int>(); // Lista para almacenar los valores de la secuencia
            while (i != end)
            {
                if (buffer[i] == 0x28) // Comprueba si el valor actual es 28
                {
                    startCounting = true; // Comienza a contar desde aquí
                    count = 1; // Inicia el contador incluyendo este 28
                    sequenceBuilder.Clear(); // Limpia cualquier secuencia previa
                    values.Add(buffer[i]); // Agrega el 28 actual a la lista
                    sequenceBuilder.Append($"{buffer[i]:X2} "); // Agrega el 28 actual a la secuencia//sequenceBuilder.Append($"{buffer[i]:D} ");
                }
                else if (startCounting)
                {
                    sequenceBuilder.Append($"{buffer[i]:X2} ");
                    values.Add(buffer[i]);
                    //sequenceBuilder.Append($"{buffer[i]:D} ");// Agrega el valor actual a la secuencia
                    count++; // Incrementa el contador por cada valor
                    if (buffer[i] == 0xE5 && count == 20) // Verifica si el valor actual es E5 y si se han contado 20 valores
                    {
                        if (values.Count >= 12) // Verifica si hay al menos 12 valores en la secuencia
                        { Console.WriteLine($"{values[11]:D}"); // Imprime el valor 12 en formato decimal
                            fullString.Add($"{values[11]:D}");
                        }//fullString.Add(sequenceBuilder.ToString());
                        //Console.WriteLine(sequenceBuilder); // Imprime la secuencia
                        break; // Sale del bucle
                    }
                } i = (i + 1) % ringsize;
            }
            int processedBytes = (end >= start) ? end - start : (ringsize - start) + end;
            return processedBytes;
        }
        static void StartRequests()
        {
            while (continueReading)// Asumiendo que la condición es siempre verdadera
            {
                ushort cksum = 0;
                byte[] buffer = new byte[20];
                buffer[0] = 0xe5;
                buffer[1] = 0x27;
                buffer[2] = 0x2;
                buffer[3] = 0x0;
                buffer[4] = 0x16;
                buffer[5] = (byte)CurrentCard;
                buffer[6] = 0;
                buffer[7] = 0x0;
                for (int i = 0; i < 8; i++)
                {cksum += buffer[i];}
                buffer[8] = (byte)((~cksum + 1) & 0xff);
                buffer[9] = (byte)(((~cksum + 1) >> 8) & 0xff);
                serialPort.Write(buffer, 0, 10);
                Thread.Sleep(20); // Ajusta este valor según sea necesario
            }
        }

    }
}