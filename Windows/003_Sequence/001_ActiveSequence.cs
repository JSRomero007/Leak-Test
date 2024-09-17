using LeakInterface.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

using System.IO;

using System.Text.RegularExpressions;


namespace LeakInterface.Windows._003_Sequence
{
    public partial class ActiveSeq : Form
    {
        private Label[] dataLabels;
        private List<string[]> dataList = new List<string[]>();
        private int currentIndex = 0;

        public ActiveSeq()
        {
            InitializeComponent();
            dataLabels = new Label[] { Sensor, Name, Dwell, Threshold, SettleValue, Minimun, ThreshTime, Final };
        }


        private void ActiveSequence_Load(object sender, EventArgs e)
        {
            ExtractIni();
        }
        private void ExtractIni()
        {

            //--- Save string in GlobalV ---\\
            try
            {

                IniFileReader ini = new IniFileReader(GlobalL.ASequence);
                string full = ini.ReadValue("Config", "Zcode");
                GlobalV.Asq = "ZXXXX" + full.Substring(5).Trim();

                CsvSearch(GlobalV.Asq);

            }
            catch (Exception ex)
            {

            }
        }
        //DBSequence
        private void CsvSearch(string targetId)
        {
            int targetColumn = 4;
            try
            {
                using (FileStream stream = new FileStream(GlobalL.DBSequence, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(','); // Asume que las columnas están separadas por comas
                        if (fields.Length > targetColumn && fields[targetColumn].Trim() == targetId)
                        {
                            DisplayFields(fields);
                            break; // Rompe el bucle una vez que encuentres el ID
                        }
                    }
                }
            }
            catch { }
        }

        private void DisplayFields(string[] fields)
        {
            try{   Label[] labels = { IdSeq, Programer, Date, Hour, ZCode, Version, NameSeq, Discart, Nodes };

            for (int i = 0; i < fields.Length && i < labels.Length; i++)
            {
                labels[i].Text = fields[i].Trim();
            }

            if (fields.Length > labels.Length)
            {
                string[] segments = fields[labels.Length].Trim('|').Split('|');

                dataList.Clear(); // Limpia la lista anterior para nuevos datos.
                foreach (var segment in segments)
                {
                    string[] dataItems = segment.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    dataList.Add(dataItems);
                }

                currentIndex = 0; // Reinicia el índice para mostrar desde el primer segmento.
                UpdateDisplay();
            }          }
 
          
            catch {
                // Limpiar todas las etiquetas si ocurre un error
                Label[] labels = { IdSeq, Programer, Date, Hour, ZCode, Version, NameSeq, Discart, Nodes, Sensor, Name, Dwell, Threshold, SettleValue, Minimun, ThreshTime, Final };
                foreach (Label label in labels)
                {
                    if (label != null)
                        label.Text = ""; // Establecer texto de la etiqueta a cadena vacía
                }

                LabelTo.Text = "";
            }
        }
        private void UpdateDataLabels(Label[] dataLabels, string[] dataItems)
        {
            for (int i = 0; i < dataItems.Length && i < dataLabels.Length; i++)
            {
                dataLabels[i].Text = dataItems[i];
            }

            // Limpiar los labels que no se usan en este conjunto de datos
            for (int i = dataItems.Length; i < dataLabels.Length; i++)
            {
                dataLabels[i].Text = "";
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex < dataList.Count - 1)
            {
                currentIndex++;
                UpdateDisplay();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            if (currentIndex < dataList.Count && dataLabels.Length > 0)
            {
                string[] currentData = dataList[currentIndex];
                for (int i = 0; i < currentData.Length && i < dataLabels.Length; i++)
                {
                    dataLabels[i].Text = currentData[i];
                }

                // Limpiar los labels que no se usan en este conjunto de datos
                for (int i = currentData.Length; i < dataLabels.Length; i++)
                {
                    dataLabels[i].Text = "";
                }

                // Actualizar la etiqueta de posición de datos
                LabelTo.Text = $"{currentIndex + 1} of {dataList.Count}";
            }
        }

    }

    public class IniFileReader
    {
        private string _path;

        public IniFileReader(string path)
        {
            _path = path;
        }

        public string ReadValue(string section, string key)
        {
            // Intenta abrir el archivo con permisos para que otros también puedan leer/escribir
            using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                bool sectionFound = false;

                while ((line = reader.ReadLine()) != null)
                {
                    // Verifica si la línea es la sección que buscamos
                    if (line.Trim().StartsWith("[") && line.Trim().EndsWith("]") &&
                        line.Trim().Equals($"[{section}]", StringComparison.OrdinalIgnoreCase))
                    {
                        sectionFound = true;
                        continue;
                    }

                    // Si la sección fue encontrada, busca la clave
                    if (sectionFound && line.Contains('='))
                    {
                        var keyValuePair = line.Split(new char[] { '=' }, 2);
                        if (keyValuePair[0].Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                        {
                            return keyValuePair[1].Trim();
                        }
                    }

                    // Si encuentra otra sección, detiene la búsqueda
                    if (sectionFound && line.Trim().StartsWith("[") && line.Trim().EndsWith("]"))
                        break;
                }
            }

            return null; // No encontrado
        }
    }
}
