using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resorter {
    public class ChoseComPortDialog : Form {
        public ChoseComPortDialog() {
            InitializeComponent();
            this.comDropDown.Items.AddRange(SerialPort.GetPortNames());
            try {
                this.comDropDown.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        public ComboBox comDropDown;
        private Button button;
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.comDropDown = new System.Windows.Forms.ComboBox();
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comDropDown
            // 
            this.comDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comDropDown.FormattingEnabled = true;
            this.comDropDown.Location = new System.Drawing.Point(25, 29);
            this.comDropDown.Name = "comDropDown";
            this.comDropDown.Size = new System.Drawing.Size(121, 24);
            this.comDropDown.TabIndex = 0;
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(25, 79);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(75, 23);
            this.button.TabIndex = 1;
            this.button.Text = "Ok";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // ChoseComPornDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 148);
            this.Controls.Add(this.button);
            this.Controls.Add(this.comDropDown);
            this.Name = "ChoseComPornDialog";
            this.Text = "ChoseComPornDialog";
            this.Load += new System.EventHandler(this.ChoseComPortDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private void button_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void ChoseComPortDialog_Load(object sender, EventArgs e) {
            if(comDropDown.Items.Count == 1) {
                this.Close();
            }
        }
    }
}
