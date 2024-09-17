namespace LeakInterface.Windows._005_Security
{
    partial class Log
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelV = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CorrectLevel = new System.Windows.Forms.Label();
            this.CorrectArea = new System.Windows.Forms.Label();
            this.UserLevelError = new System.Windows.Forms.Label();
            this.UserAreaError = new System.Windows.Forms.Label();
            this.UserNameError = new System.Windows.Forms.Label();
            this.ExampleName = new System.Windows.Forms.Label();
            this.UserLevel = new System.Windows.Forms.ComboBox();
            this.UserArea = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LoginKey = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UserName = new System.Windows.Forms.TextBox();
            this.NewPassword = new System.Windows.Forms.PictureBox();
            this.UserDataTable = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.AddUser = new CustomControls.RJControls.RJButton();
            this.UpdateUser = new CustomControls.RJControls.RJButton();
            this.DeleteUser = new CustomControls.RJControls.RJButton();
            this.Cancel = new CustomControls.RJControls.RJButton();
            this.rjButton1 = new CustomControls.RJControls.RJButton();
            this.StatusButtons = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserDataTable)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.labelV);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(-3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1075, 32);
            this.panel1.TabIndex = 8;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::LeakInterface.Properties.Resources.ico1;
            this.pictureBox2.Location = new System.Drawing.Point(5, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // labelV
            // 
            this.labelV.AutoSize = true;
            this.labelV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelV.ForeColor = System.Drawing.Color.Silver;
            this.labelV.Location = new System.Drawing.Point(133, 6);
            this.labelV.Name = "labelV";
            this.labelV.Size = new System.Drawing.Size(51, 20);
            this.labelV.TabIndex = 6;
            this.labelV.Text = "label9";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Leak Control";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.pictureBox3);
            this.panel2.Location = new System.Drawing.Point(-3, 505);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1075, 50);
            this.panel2.TabIndex = 7;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox3.Image = global::LeakInterface.Properties.Resources.Aptiv_logo;
            this.pictureBox3.Location = new System.Drawing.Point(433, -1);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(243, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(34, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Super User";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CorrectLevel);
            this.groupBox1.Controls.Add(this.CorrectArea);
            this.groupBox1.Controls.Add(this.UserLevelError);
            this.groupBox1.Controls.Add(this.UserAreaError);
            this.groupBox1.Controls.Add(this.UserNameError);
            this.groupBox1.Controls.Add(this.ExampleName);
            this.groupBox1.Controls.Add(this.UserLevel);
            this.groupBox1.Controls.Add(this.UserArea);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.LoginKey);
            this.groupBox1.Controls.Add(this.ID);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.UserName);
            this.groupBox1.Controls.Add(this.NewPassword);
            this.groupBox1.Location = new System.Drawing.Point(12, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1045, 100);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // CorrectLevel
            // 
            this.CorrectLevel.AutoSize = true;
            this.CorrectLevel.ForeColor = System.Drawing.Color.Gray;
            this.CorrectLevel.Location = new System.Drawing.Point(726, 41);
            this.CorrectLevel.Name = "CorrectLevel";
            this.CorrectLevel.Size = new System.Drawing.Size(114, 13);
            this.CorrectLevel.TabIndex = 25;
            this.CorrectLevel.Text = "Select a correct option\n";
            // 
            // CorrectArea
            // 
            this.CorrectArea.AutoSize = true;
            this.CorrectArea.ForeColor = System.Drawing.Color.Gray;
            this.CorrectArea.Location = new System.Drawing.Point(458, 41);
            this.CorrectArea.Name = "CorrectArea";
            this.CorrectArea.Size = new System.Drawing.Size(114, 13);
            this.CorrectArea.TabIndex = 25;
            this.CorrectArea.Text = "Select a correct option\n";
            // 
            // UserLevelError
            // 
            this.UserLevelError.AutoSize = true;
            this.UserLevelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserLevelError.ForeColor = System.Drawing.Color.Red;
            this.UserLevelError.Location = new System.Drawing.Point(644, 18);
            this.UserLevelError.Name = "UserLevelError";
            this.UserLevelError.Size = new System.Drawing.Size(17, 24);
            this.UserLevelError.TabIndex = 24;
            this.UserLevelError.Text = "*";
            // 
            // UserAreaError
            // 
            this.UserAreaError.AutoSize = true;
            this.UserAreaError.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserAreaError.ForeColor = System.Drawing.Color.Red;
            this.UserAreaError.Location = new System.Drawing.Point(435, 22);
            this.UserAreaError.Name = "UserAreaError";
            this.UserAreaError.Size = new System.Drawing.Size(17, 24);
            this.UserAreaError.TabIndex = 24;
            this.UserAreaError.Text = "*";
            // 
            // UserNameError
            // 
            this.UserNameError.AutoSize = true;
            this.UserNameError.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameError.ForeColor = System.Drawing.Color.Red;
            this.UserNameError.Location = new System.Drawing.Point(233, 19);
            this.UserNameError.Name = "UserNameError";
            this.UserNameError.Size = new System.Drawing.Size(17, 24);
            this.UserNameError.TabIndex = 24;
            this.UserNameError.Text = "*";
            // 
            // ExampleName
            // 
            this.ExampleName.AutoSize = true;
            this.ExampleName.ForeColor = System.Drawing.Color.Gray;
            this.ExampleName.Location = new System.Drawing.Point(241, 41);
            this.ExampleName.Name = "ExampleName";
            this.ExampleName.Size = new System.Drawing.Size(115, 13);
            this.ExampleName.TabIndex = 23;
            this.ExampleName.Text = "Example: Jose Romero";
            // 
            // UserLevel
            // 
            this.UserLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.UserLevel.FormattingEnabled = true;
            this.UserLevel.Location = new System.Drawing.Point(596, 57);
            this.UserLevel.Name = "UserLevel";
            this.UserLevel.Size = new System.Drawing.Size(244, 32);
            this.UserLevel.TabIndex = 22;
            this.UserLevel.TextChanged += new System.EventHandler(this.UserLevel_TextChanged);
            // 
            // UserArea
            // 
            this.UserArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.UserArea.FormattingEnabled = true;
            this.UserArea.Location = new System.Drawing.Point(392, 57);
            this.UserArea.Name = "UserArea";
            this.UserArea.Size = new System.Drawing.Size(182, 32);
            this.UserArea.TabIndex = 21;
            this.UserArea.TextChanged += new System.EventHandler(this.UserArea_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label13.Location = new System.Drawing.Point(846, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(9, 78);
            this.label13.TabIndex = 20;
            this.label13.Text = "|\r\n|\r\n|\r\n|\r\n|\r\n|";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label12.Location = new System.Drawing.Point(580, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(9, 78);
            this.label12.TabIndex = 20;
            this.label12.Text = "|\r\n|\r\n|\r\n|\r\n|\r\n|";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label11.Location = new System.Drawing.Point(377, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(9, 78);
            this.label11.TabIndex = 20;
            this.label11.Text = "|\r\n|\r\n|\r\n|\r\n|\r\n|";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label10.Location = new System.Drawing.Point(133, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(9, 78);
            this.label10.TabIndex = 20;
            this.label10.Text = "|\r\n|\r\n|\r\n|\r\n|\r\n|";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(595, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "Level:";
            // 
            // LoginKey
            // 
            this.LoginKey.AutoSize = true;
            this.LoginKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginKey.ForeColor = System.Drawing.Color.Gray;
            this.LoginKey.Location = new System.Drawing.Point(870, 52);
            this.LoginKey.Name = "LoginKey";
            this.LoginKey.Size = new System.Drawing.Size(76, 25);
            this.LoginKey.TabIndex = 17;
            this.LoginKey.Text = "label8";
            this.LoginKey.TextChanged += new System.EventHandler(this.LoginKey_TextChanged);
            // 
            // ID
            // 
            this.ID.AutoSize = true;
            this.ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ID.ForeColor = System.Drawing.Color.Gray;
            this.ID.Location = new System.Drawing.Point(33, 57);
            this.ID.Name = "ID";
            this.ID.Size = new System.Drawing.Size(51, 20);
            this.ID.TabIndex = 16;
            this.ID.Text = "label7";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(871, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Login Code:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(392, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Area:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(144, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "User name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "ID:";
            // 
            // UserName
            // 
            this.UserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserName.Location = new System.Drawing.Point(148, 57);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(223, 29);
            this.UserName.TabIndex = 11;
            this.UserName.TextChanged += new System.EventHandler(this.UserName_TextChanged);
            // 
            // NewPassword
            // 
            this.NewPassword.Image = global::LeakInterface.Properties.Resources.NEWPassword2;
            this.NewPassword.Location = new System.Drawing.Point(952, 34);
            this.NewPassword.Name = "NewPassword";
            this.NewPassword.Size = new System.Drawing.Size(53, 55);
            this.NewPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.NewPassword.TabIndex = 12;
            this.NewPassword.TabStop = false;
            this.NewPassword.Click += new System.EventHandler(this.NewPassword_Click);
            // 
            // UserDataTable
            // 
            this.UserDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserDataTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UserDataTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.UserDataTable.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.UserDataTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UserDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UserDataTable.Location = new System.Drawing.Point(12, 270);
            this.UserDataTable.MultiSelect = false;
            this.UserDataTable.Name = "UserDataTable";
            this.UserDataTable.ReadOnly = true;
            this.UserDataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UserDataTable.Size = new System.Drawing.Size(1045, 217);
            this.UserDataTable.TabIndex = 17;
            this.UserDataTable.SelectionChanged += new System.EventHandler(this.UserDataTable_SelectionChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.AddUser);
            this.flowLayoutPanel1.Controls.Add(this.UpdateUser);
            this.flowLayoutPanel1.Controls.Add(this.DeleteUser);
            this.flowLayoutPanel1.Controls.Add(this.Cancel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(430, 205);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(631, 49);
            this.flowLayoutPanel1.TabIndex = 18;
            // 
            // AddUser
            // 
            this.AddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(66)))));
            this.AddUser.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(66)))));
            this.AddUser.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.AddUser.BorderRadius = 5;
            this.AddUser.BorderSize = 0;
            this.AddUser.FlatAppearance.BorderSize = 0;
            this.AddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddUser.ForeColor = System.Drawing.Color.White;
            this.AddUser.Location = new System.Drawing.Point(478, 3);
            this.AddUser.Name = "AddUser";
            this.AddUser.Size = new System.Drawing.Size(150, 40);
            this.AddUser.TabIndex = 13;
            this.AddUser.Text = "Add User";
            this.AddUser.TextColor = System.Drawing.Color.White;
            this.AddUser.UseVisualStyleBackColor = false;
            this.AddUser.Click += new System.EventHandler(this.AddUser_Click);
            // 
            // UpdateUser
            // 
            this.UpdateUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.UpdateUser.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.UpdateUser.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.UpdateUser.BorderRadius = 5;
            this.UpdateUser.BorderSize = 0;
            this.UpdateUser.FlatAppearance.BorderSize = 0;
            this.UpdateUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateUser.ForeColor = System.Drawing.Color.White;
            this.UpdateUser.Location = new System.Drawing.Point(322, 3);
            this.UpdateUser.Name = "UpdateUser";
            this.UpdateUser.Size = new System.Drawing.Size(150, 40);
            this.UpdateUser.TabIndex = 15;
            this.UpdateUser.Text = "Update User";
            this.UpdateUser.TextColor = System.Drawing.Color.White;
            this.UpdateUser.UseVisualStyleBackColor = false;
            this.UpdateUser.Click += new System.EventHandler(this.UpdateUser_Click);
            // 
            // DeleteUser
            // 
            this.DeleteUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(52)))), ((int)(((byte)(68)))));
            this.DeleteUser.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(52)))), ((int)(((byte)(68)))));
            this.DeleteUser.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.DeleteUser.BorderRadius = 5;
            this.DeleteUser.BorderSize = 0;
            this.DeleteUser.FlatAppearance.BorderSize = 0;
            this.DeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteUser.ForeColor = System.Drawing.Color.White;
            this.DeleteUser.Location = new System.Drawing.Point(166, 3);
            this.DeleteUser.Name = "DeleteUser";
            this.DeleteUser.Size = new System.Drawing.Size(150, 40);
            this.DeleteUser.TabIndex = 14;
            this.DeleteUser.Text = "Delete User";
            this.DeleteUser.TextColor = System.Drawing.Color.White;
            this.DeleteUser.UseVisualStyleBackColor = false;
            this.DeleteUser.Click += new System.EventHandler(this.DeleteUser_Click);
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.Gray;
            this.Cancel.BackgroundColor = System.Drawing.Color.Gray;
            this.Cancel.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Cancel.BorderRadius = 5;
            this.Cancel.BorderSize = 0;
            this.Cancel.FlatAppearance.BorderSize = 0;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.ForeColor = System.Drawing.Color.White;
            this.Cancel.Location = new System.Drawing.Point(10, 3);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(150, 40);
            this.Cancel.TabIndex = 16;
            this.Cancel.Text = "Cancel";
            this.Cancel.TextColor = System.Drawing.Color.White;
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(52)))), ((int)(((byte)(68)))));
            this.rjButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(52)))), ((int)(((byte)(68)))));
            this.rjButton1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rjButton1.BorderRadius = 5;
            this.rjButton1.BorderSize = 0;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton1.ForeColor = System.Drawing.Color.Transparent;
            this.rjButton1.Location = new System.Drawing.Point(12, 38);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(119, 35);
            this.rjButton1.TabIndex = 16;
            this.rjButton1.Text = "<--- Exit";
            this.rjButton1.TextColor = System.Drawing.Color.Transparent;
            this.rjButton1.UseVisualStyleBackColor = false;
            this.rjButton1.Click += new System.EventHandler(this.rjButton1_Click);
            // 
            // StatusButtons
            // 
            this.StatusButtons.AutoSize = true;
            this.StatusButtons.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusButtons.Location = new System.Drawing.Point(8, 247);
            this.StatusButtons.Name = "StatusButtons";
            this.StatusButtons.Size = new System.Drawing.Size(51, 20);
            this.StatusButtons.TabIndex = 19;
            this.StatusButtons.Text = "label7";
            // 
            // Log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1069, 554);
            this.Controls.Add(this.StatusButtons);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.UserDataTable);
            this.Controls.Add(this.rjButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Log";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log";
            this.Load += new System.EventHandler(this.Log_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserDataTable)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox UserName;
        private CustomControls.RJControls.RJButton AddUser;
        private CustomControls.RJControls.RJButton DeleteUser;
        private CustomControls.RJControls.RJButton UpdateUser;
        private System.Windows.Forms.Label LoginKey;
        private System.Windows.Forms.Label ID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private CustomControls.RJControls.RJButton rjButton1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView UserDataTable;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox NewPassword;
        private System.Windows.Forms.ComboBox UserArea;
        private System.Windows.Forms.ComboBox UserLevel;
        private System.Windows.Forms.Label ExampleName;
        private System.Windows.Forms.Label UserNameError;
        private System.Windows.Forms.Label UserLevelError;
        private System.Windows.Forms.Label UserAreaError;
        private System.Windows.Forms.Label CorrectArea;
        private System.Windows.Forms.Label CorrectLevel;
        private CustomControls.RJControls.RJButton Cancel;
        private System.Windows.Forms.Label StatusButtons;
    }
}