namespace LeakInterface
{
    partial class PopState
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
            this.label1 = new System.Windows.Forms.Label();
            this.OnOffbtn = new CustomControls.RJControls.RJToggleButton();
            this.LeakIsDisable = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LeakIsDisable)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(74, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Leak";
            // 
            // OnOffbtn
            // 
            this.OnOffbtn.Enabled = false;
            this.OnOffbtn.Location = new System.Drawing.Point(11, 8);
            this.OnOffbtn.MinimumSize = new System.Drawing.Size(45, 22);
            this.OnOffbtn.Name = "OnOffbtn";
            this.OnOffbtn.OffBackColor = System.Drawing.Color.Black;
            this.OnOffbtn.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.OnOffbtn.OnBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(62)))), ((int)(((byte)(12)))));
            this.OnOffbtn.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.OnOffbtn.Size = new System.Drawing.Size(57, 33);
            this.OnOffbtn.TabIndex = 1;
            this.OnOffbtn.UseVisualStyleBackColor = true;
            // 
            // LeakIsDisable
            // 
            this.LeakIsDisable.Image = global::LeakInterface.Properties.Resources.kDHYq070Bo;
            this.LeakIsDisable.Location = new System.Drawing.Point(126, -1);
            this.LeakIsDisable.Name = "LeakIsDisable";
            this.LeakIsDisable.Size = new System.Drawing.Size(25, 25);
            this.LeakIsDisable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LeakIsDisable.TabIndex = 2;
            this.LeakIsDisable.TabStop = false;
            // 
            // State
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(153, 48);
            this.Controls.Add(this.OnOffbtn);
            this.Controls.Add(this.LeakIsDisable);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(880, 0);
            this.Name = "State";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "State";
            this.Load += new System.EventHandler(this.State_Load);
            this.DoubleClick += new System.EventHandler(this.State_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.LeakIsDisable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CustomControls.RJControls.RJToggleButton OnOffbtn;
        private System.Windows.Forms.PictureBox LeakIsDisable;
    }
}