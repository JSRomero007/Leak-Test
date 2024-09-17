using LeakInterface.CSVRead;
using LeakInterface.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace LeakInterface
{
    /*
  *   private static string path = "C:\\Users\\y80txk\\Documents\\LeakConfig\\Log.txt";
        public static void LogEvent(string codigo, string fecha, string hora, string turno, string operador)
        {
            // Verifica la cantidad de registros existentes
            int registroCount = File.ReadLines(path).Where(line => line.Contains("Información capturada en la base de datos")).Count();

            // Si hay 100 registros, limpia el archivo y comienza de nuevo
            if (registroCount >= 100)
            {
                File.WriteAllText(path, string.Empty);
            }

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("┌────────────────────────────────────────────────────┐");
                writer.WriteLine("│     Información capturada en la base de datos      │");
                writer.WriteLine("│────────────────────────────────────────────────────│");
                writer.WriteLine("│                                                    │");
                writer.WriteLine("│                                                    │");
                writer.WriteLine($"│▪ Código z capturado : {codigo}");
                writer.WriteLine($"│▪ Fecha de del registro : {fecha}");
                writer.WriteLine($"│▪ Hora del registro : {hora}");
                writer.WriteLine($"│▪ Turno: {turno}");
                writer.WriteLine($"│▪ Operador que realizo el registro : {operador}");
                writer.WriteLine("└────────────────────────────────────────────────────┘");
                writer.WriteLine();
            }
        }
  * 
  *///This code save register 

    public partial class User : Form
    {
        private static string path = GlobalL.L__Security;
        public class UsuarioInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Area { get; set; }
            public string Level { get; set; }
        }
        internal string Area;
        internal string Code;
        internal string Level;
        string USRArea, USRLevel;
        string Hour = DateTime.Now.ToString("hh:mm tt");
        string Date = DateTime.Now.ToString("D");

        public User()
        {
            InitializeComponent();
            GlobalXml.XmlReader.LoadUsers(GlobalL.UserConfig);
            AutoLoad();
        }
        private void AutoLoad()
        {
            GC.Collect();
            ErrorUser.Text = string.Empty;
            labelV.Text = Global.GlobalV.Version.ToString();
        }

        //--- Exit Btn ---\\
        private void rjButton2_Click(object sender, EventArgs e)
        {
            GC.Collect();
            this.Close();
        }


        private void Login_Click(object sender, EventArgs e)
        {

            string code = CodeBox.Text.Trim();
            if (GlobalXml.GlobalDU.usr.TryGetValue(code, out GlobalXml.User user))
            {
                GlobalV.LogId = code;
                GlobalV.LogUser = user.Name;
                //--- SAVE data login ---\\
                USRArea = user.Area;
                USRLevel = user.Level;
                // Print All information.
                //Console.WriteLine($"Nombre: {user.Name}, Área: {user.Area}, Nivel: {user.Level}");
                switch (user.Level)
                {
                    case "Aptiv-Admin-log"://Aptiv-KWS10040-ULE //User-Elevate-Level
                        OpenModeAdminUser();
                        break;
                    case "Aptiv-Level1-ULS"://Aptiv-Level1-ULS  //User-Status-Level
                        GlobalT.UserLevel = "UserLevelStatus_1_ReadMode";
                        OpenFullAcces();
                        break;
                    case "Aptiv-Level2-ULS"://Aptiv-Level1-ULS  //User-Status-Level
                        GlobalT.UserLevel = "UserLevelStatus_2_ReadAndWriteMode";
                        OpenFullAcces();
                        break;
                    case "Aptiv-Level3-ULS":
                        GlobalT.UserLevel = "UserLevelStatus_3_FullAccessMode";
                        OpenFullAcces();
                        break;
                    case "Aptiv-Develop-UEL"://Aptiv-Level1-UEL 
                        GlobalT.UserLevel = "UserLevelStatus_develop_Key1004098ws";
                        OpenFullAcces();
                        break;
                    default:
                        ErrorUser.Text = "Pleace type the Correct Code to continue.";
                        break;
                }
                ErrorUser.Text = string.Empty;

            }
            else
            {
                ErrorUser.Text = "User not Found :(";
                ErrorUser.ForeColor = Color.Red;
            }
        }
        //--- Open Mode Addmin (Add delete edit Users) ---\\
        private void OpenModeAdminUser()
        {
            AccessLogin();
            
            GC.Collect();
            this.Hide();
            Windows._005_Security.Log Next_User = new Windows._005_Security.Log();
            Next_User.ShowDialog();
            this.Show();

        }
        //---- Fullacces Window ----\\
        private void OpenFullAcces()
        {
            AccessLogin();
            GC.Collect();
            this.Hide();
            FullAcces Next_User = new FullAcces();
            Next_User.ShowDialog();
            this.Show();

        }

        private void AccessLogin()
        {
            CodeBox.Text =string.Empty; 
            LoginUserLogEvent(GlobalV.LogId, GlobalV.LogUser, USRArea, USRLevel, Hour, Date);
        }

        //--- Create LOG
        public static void LoginUserLogEvent(string UserLog, string NameLog, string AreaLog, string LevelLog, string Hour, string Date)
        {

            int registroCount = File.ReadLines(path).Where(line => line.Contains("┌────────────| login user |────────────┐")).Count();

            //--- Clear log if register log is >= 300
            if (registroCount >= 300)
            {
                File.WriteAllText(path, string.Empty);
            }

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("┌────────────| login user |────────────┐");
                writer.WriteLine("│                                      │");
                writer.WriteLine($"│● Code User: {UserLog}");
                writer.WriteLine($"│● Name:{NameLog}");
                writer.WriteLine($"│● Area: {AreaLog}");
                writer.WriteLine($"│● Level: {LevelLog}");
                writer.WriteLine($"│● Hour: {Hour}");
                writer.WriteLine($"│● Date:{Date}");
                writer.WriteLine("└──────────────────────────────────────┘");
                writer.WriteLine();
            }
        }


    }
}
