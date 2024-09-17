using LeakInterface.Global;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.MonthCalendar;
namespace LeakInterface.Windows._005_Security
{
    public partial class Log : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static IntPtr SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);
        //--- Mouse Windows ---\
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); }

        private static readonly Random _randomKey = new Random();
        private static readonly Random _randomId = new Random();
        private const string CharacterKey = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const string CharacterID = "ABCDEFZ0123456789";
        public Log()
        {
            InitializeComponent();
            LoadAreaItems();
            LoadLevelItems();
            labelV.Text = GlobalV.Version;
            StatusButtons.Visible = false;
        }
        //--- Add items in Area ---\\
        private void LoadAreaItems()
        {
            UserArea.Items.Clear();
            UserArea.Items.Add("");
            UserArea.Items.Add("Operator");
            UserArea.Items.Add("Technician");
            UserArea.Items.Add("Engineer");
            UserArea.Items.Add("Administrator");
        }
        //--- Add items in Levels ---\\
        private void LoadLevelItems()
        {
            UserLevel.Items.Clear();
            UserLevel.Items.Add("");
            UserLevel.Items.Add("Level 1 -view");
            UserLevel.Items.Add("Level 2 -View and write");
            UserLevel.Items.Add("Level 3 -Full access");
        }

        Regex Userarea = new Regex("^(Operator|Technician|Engineer|Administrator)$");
        private void UserArea_TextChanged(object sender, EventArgs e)
        {
            GlobalT.UserArea_Cng = UserArea.Text;

            if (Userarea.IsMatch(UserArea.Text))
            {
                UserLevel.Enabled = true;
                CorrectArea.Visible = false;
                UserAreaError.Visible = false;
            }
            else
            {
                UserLevel.Enabled = false;
                UserAreaError.Visible = true;
                CorrectArea.Visible = true;
                NewPassword.Enabled = false;
            }
        }


        Regex Userlevel = new Regex("^(Level 1 -view|Level 2 -View and write|Level 3 -Full access)");
        private void UserLevel_TextChanged(object sender, EventArgs e)
        {
            GlobalT.UserLevel_Cng = UserLevel.Text;
            if (Userlevel.IsMatch(UserLevel.Text))
            {
                switch (UserLevel.Text)
                {
                    case "Level 1 -view":
                        GlobalT.LevelStatus = "Aptiv-Level1-ULS";
                        break;
                    case "Level 2 -View and write":
                        GlobalT.LevelStatus = "Aptiv-Level2-ULS";
                        break;
                    case "Level 3 -Full access":
                        GlobalT.LevelStatus = "Aptiv-Level3-ULS";
                        break;
                    default:
                        GlobalT.LevelStatus = "Aptiv-Level1-ULS";
                        break;
                }
                NewPassword.Enabled = true;
                UserLevelError.Visible = false;
                CorrectArea.Visible = false;
            }
            else
            {
                CorrectLevel.Visible = false;
                UserLevelError.Visible = true;
                LoginKey.Text = string.Empty;
                NewPassword.Enabled = false;

            }

        }

        public static string UserCode(int longitud)
        {
            StringBuilder resultado = new StringBuilder(longitud);

            for (int i = 0; i < longitud; i++)
            {
                char caracterAleatorio = CharacterKey[_randomKey.Next(CharacterKey.Length)];
                resultado.Append(caracterAleatorio);
            }

            return resultado.ToString();
        }
        public static string Idcode(int longitud)
        {
            StringBuilder resultado = new StringBuilder(longitud);

            for (int i = 0; i < longitud; i++)
            {
                char caracterAleatorio = CharacterID[_randomId.Next(CharacterID.Length)];
                resultado.Append(caracterAleatorio);
            }

            return resultado.ToString();
        }



        private void rjButton1_Click(object sender, EventArgs e)
        {
            GC.Collect();//---Clear Data---\\
            this.Close();
        }
        private void XmlDataUsers()
        {
            // Create a DataSet
            DataSet dataSet = new DataSet();

            try
            {
                dataSet.ReadXml(GlobalL.UserConfig);

                // Asegurarse de que tenemos datos antes de intentar asignarlos al DataGridView.
                if (dataSet.Tables.Count > 0)
                {
                    DataTable dt = dataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        // Lista para guardar las filas que serán eliminadas.
                        List<DataRow> rowsToDelete = new List<DataRow>();

                        foreach (DataRow row in dt.Rows)
                        {
                            // Verifica alguna condición. Aquí uso un ejemplo genérico.
                            // Puedes adaptarlo a tu caso específico.
                            if (row["Level"].ToString() == "Aptiv-Develop-UEL"||row["Level"].ToString() == "Aptiv-Admin-log")
                            {
                                rowsToDelete.Add(row);
                            }
                        }

                        // Eliminar las filas identificadas.
                        foreach (DataRow row in rowsToDelete)
                        {
                            dt.Rows.Remove(row);
                        }

                        UserDataTable.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("No hay datos para mostrar.");
                    }
                }
                else
                {
                    MessageBox.Show("No hay datos para mostrar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Log_Load(object sender, EventArgs e)
        {
            Dicionary();
            ExtractIds();
            // LoadComItems();
            XmlDataUsers();
            //---- Label Errors ----\\
            ExampleName.Visible = false;
            UserAreaError.Visible = false;
            UserLevelError.Visible = false;

            //---- Buttons ----\\
            DeleteUser.Visible = false;
            UpdateUser.Visible = false;
            AddUser.Visible = false;
            Cancel.Visible = false;
            NewPassword.Enabled = false;
            LoginKey.Text = string.Empty;
            ID.Text = string.Empty;
            AddUser.Visible = false;
            //---- Combobox ----\\
            UserArea.Enabled = false;
            UserLevel.Enabled = false;
            //---- TextBox ----\\
            UserName.Enabled = false;

        }

        private void DataCell()
        {
            DataGridViewRow selectedRow = UserDataTable.SelectedRows[0];
            string valFile = selectedRow.Cells[4].Value.ToString();

            string Name = selectedRow.Cells[0].Value.ToString();
            string Area = selectedRow.Cells[1].Value.ToString();
            string Code = selectedRow.Cells[2].Value.ToString();
            string Level = selectedRow.Cells[3].Value.ToString();
            DeleteUser.Visible = true;

            UpdateUser.Visible = true;
            AddUser.Visible = false;
            //--- Save colum selection ---\\ 
            ID.Text = valFile.ToString();
            GlobalT.SelectedID = valFile;
            //--- Name ---\\
            UserName.Text = Name;
            UserArea.SelectedItem = Area;

            if (Level == "Aptiv-Level1-ULS")
            { UserLevel.SelectedItem = "Level 1 -view"; }

            else if (Level == "Aptiv-Level2-ULS")
            { UserLevel.SelectedItem = "Level 2 -View and write"; }

            else if (Level == "Aptiv-Level3-ULS")
            { UserLevel.SelectedItem = "Level 3 -Full access"; }

            else
            { UserLevel.SelectedItem = ""; }

            GlobalT.AccessCode_Cng = Code;
            LoginKey.Text = Code;

        }
        private void ClearCellORAddNewUser()
        {
            GlobalT.SelectedID = string.Empty;
            UserArea.SelectedIndex = 0;
            UserLevel.SelectedIndex = 0;
            DeleteUser.Visible = false;
            UserLevel.Enabled = false;
            UserArea.Enabled = false;
            UpdateUser.Enabled = false;
            AddUser.Visible = true;
            UserLevelError.Visible = true;
            UserName.Text = string.Empty;
            GlobalT.RandomID = Idcode(2);
            ID.Text = GlobalT.RandomID;
            UpdateUser.Visible = false;
            AddUser.Visible = false;
        }

        private void UserDataTable_SelectionChanged(object sender, EventArgs e)
        {

            SelectionAndUSE();
        }

        private void SelectionAndUSE()
        {
            UserName.Enabled = true;

            try
            {
                DataGridViewRow selectedRow = UserDataTable.SelectedRows[0];
                string valFile = selectedRow.Cells[4].Value.ToString();

                if (valFile != "" && Name != "")
                {
                    //---- Diplay all Cells ----\\
                    DataCell();
                    string keyToFind = valFile;
                    Console.WriteLine(keyToFind);
                    // Comprobar si el diccionario contiene la clave

                    string idToFind = keyToFind; // asumiendo que tienes un TextBox para el ID



                    if (GlobalD.users.ContainsKey(idToFind))
                    {
                        User user = GlobalD.users[idToFind];

                        switch (GlobalT.UserLevel_Cng)
                        {
                            case "Level 1 -view":
                                GlobalT.ChangeLevel = "Aptiv-Level1-ULS";
                                break;
                            case "Level 2 -View and write":
                                GlobalT.ChangeLevel = "Aptiv-Level2-ULS";
                                break;
                            case "Level 3 -Full access":
                                GlobalT.ChangeLevel = "Aptiv-Level3-ULS";
                                break;
                            default:
                                GlobalT.ChangeLevel = "Aptiv-Level1-ULS";
                                break;
                        }
                        UpdateUser.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show($"No se encontró un usuario con el ID: {idToFind}");
                    }


                }
                else
                {
                    //---- Cells clear ----\\
                    ClearCellORAddNewUser();
                }

            }
            catch
            {


            }

        }
        private void NewPassword_Click(object sender, EventArgs e)
        {
            GlobalT.AccessCode_Cng = GlobalT.AccessCode;
            GlobalT.AccessCode = UserCode(5);
            LoginKey.Text = GlobalT.AccessCode;
        }

        private void UserName_TextChanged(object sender, EventArgs e)
        {
            GlobalT.UserName_Cng = UserName.Text;

            if (UserName.Text.Length >= 10)
            {
                UserArea.Enabled = true;
                ExampleName.Visible = false;
                UserNameError.Visible = false;
            }
            else
            {
                ExampleName.Visible = true;
                UserArea.Enabled = false;
                UserLevel.Enabled = false;
                NewPassword.Enabled = false;
            }

        }

     

        private void AddUser_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument xdoc;
                try
                {
                    xdoc = XDocument.Load(GlobalL.UserConfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading XML file: {ex.Message}");
                    return;
                }


                // Crear un nuevo usuario
                XElement newUser = new XElement("User",
                    new XAttribute("id", GlobalT.RandomID),
                    new XElement("Name", UserName.Text),
                    new XElement("Area", UserArea.Text),
                    new XElement("Code", GlobalT.AccessCode),
                    new XElement("Level", GlobalT.LevelStatus)
                );
                //---Add User
                xdoc.Root.Add(newUser);

                //---Save User
                try
                {
                    xdoc.Save(GlobalL.UserConfig);
                    Console.WriteLine("New user added successfully!");
                }
                catch (Exception ex)
                { Console.WriteLine($"Error saving XML file: {ex.Message}"); }

                StatusButtons.Text = $"Add new user, Complete ";
                StatusButtons.Visible = true;
                StatusButtons.ForeColor = Color.Black;

            }
            catch
            {

            }
            XmlDataUsers();
            UserDataTable.Refresh();
            ExtractIds();

        }

        private void LoginKey_TextChanged(object sender, EventArgs e)
        {
            GlobalT.AccessCode = LoginKey.Text;
            GlobalT.AccessCode_Cng = LoginKey.Text;

            if (GlobalVA.UserId.Contains(GlobalT.SelectedID))
            {
                GlobalT.AccessCode = LoginKey.Text;
            }

            else
            {
                if (GlobalT.AccessCode.Length > 4)
                {
                    GlobalT.AccessCode = LoginKey.Text;
                    AddUser.Visible = true;

                }
                else
                {
                    AddUser.Visible = false;
                }
            }


        }
        private void ExtractIds()
        {
            // Cargar el XML existente
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(GlobalL.UserConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading XML file: {e.Message}");
                return;
            }

            // Utilizar LINQ para extraer los id de los usuarios y almacenarlos en una lista
            try
            {
                GlobalVA.UserId = xdoc.Root
                              .Elements("User")
                              .Where(u => u.Attribute("id") != null)
                              .Select(u => u.Attribute("id").Value)
                              .ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error extracting user ids: {e.Message}");
                return;
            }

            // Mostrar los id extraídos en la consola
            Console.WriteLine("User IDs:");
            if (GlobalVA.UserId.Length == 0)
            {
                Console.WriteLine("No user IDs were extracted.");
            }
            else
            {
                foreach (string id in GlobalVA.UserId)
                {
                    Console.WriteLine(id);
                }
            }
        }

        private void DeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument xdoc;
                try
                {
                    xdoc = XDocument.Load(GlobalL.UserConfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading XML file: {ex.Message}");
                    return;
                }

                string userIdToDelete = GlobalT.SelectedID; // ID del usuario a eliminar

                try
                {
                    // Buscar y eliminar el usuario con el ID proporcionado
                    xdoc.Root.Elements("User")
                             .Where(u => (string)u.Attribute("id") == userIdToDelete)
                             .Remove();

                    xdoc.Save(GlobalL.UserConfig); // Guardar los cambios
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting user: {ex.Message}");
                }

                StatusButtons.Text = $"Delete user ID:{GlobalT.SelectedID}, Complete ";
                StatusButtons.Visible = true;
                StatusButtons.ForeColor = Color.Black;
            }
            catch
            {
                StatusButtons.Text = "";
                StatusButtons.Visible = true;
                StatusButtons.ForeColor = Color.Red;
            }

            XmlDataUsers();
            UserDataTable.Refresh();
            ExtractIds();
        }

        private void Dicionary()
        {


            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(GlobalL.UserConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading XML file: {e.Message}");
                return;
            }


            try
            {
                GlobalD.users = xdoc.Root.Elements("User")
                                  .ToDictionary(
                                      u => (string)u.Attribute("id"),
                                      u => new User
                                      {
                                          Name = (string)u.Element("Name"),
                                          Area = (string)u.Element("Area"),
                                          Code = (string)u.Element("Code"),
                                          Level = (string)u.Element("Level")
                                      }
                                  );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error parsing users: {e.Message}");
                return;
            }


        }

        private void UpdateUser_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument doc;
                try
                {
                    doc = XDocument.Load(GlobalL.UserConfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading XML: {ex.Message}");
                    return;
                }

                // The ID of the user to update.
                string userIdToUpdate = GlobalT.SelectedID;

                // Find the user using the ID.
                XElement user = doc.Descendants("User")
                                   .FirstOrDefault(u => u.Attribute("id")?.Value == userIdToUpdate);

                // Update the area of the found user.
                if (user != null)
                {
                    XElement userElement = user.Element("Name");
                    XElement areaElement = user.Element("Area");
                    XElement codeElement = user.Element("Code");
                    XElement leveElement = user.Element("Level");

                    if (userElement != null && areaElement != null && codeElement != null && leveElement != null)
                    {
                        userElement.Value = GlobalT.UserName_Cng;
                        areaElement.Value = GlobalT.UserArea_Cng;
                        codeElement.Value = GlobalT.AccessCode_Cng;
                        leveElement.Value = GlobalT.LevelStatus;
                    }
                }
                else
                {
                    Console.WriteLine("User not found!");
                }

                // Save the modified XML back to file.
                try
                {
                    doc.Save(GlobalL.UserConfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving XML: {ex.Message}");
                }
                StatusButtons.Text = "Update user, Complete ";
                StatusButtons.Visible = true;
                StatusButtons.ForeColor = Color.Green;
            }
            catch
            {
                StatusButtons.Text = "";
                StatusButtons.Visible = true;
                StatusButtons.ForeColor = Color.Black;
            }

            XmlDataUsers();
            UserDataTable.Refresh();
            ExtractIds();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            SelectionAndUSE();

            StatusButtons.Text = "Cancel Editing, Complete ";
            StatusButtons.Visible = true;
            StatusButtons.ForeColor = Color.Black;
        }
    }
}
