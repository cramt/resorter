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
    public class ChoseComPornDialog : Form {
        public ChoseComPornDialog() {
            InitializeComponent();
        }

        private ComboBox comDropDown;
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
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comDropDown.FormattingEnabled = true;
            this.comDropDown.Location = new System.Drawing.Point(29, 56);
            this.comDropDown.Name = "com drop down";
            this.comDropDown.Size = new System.Drawing.Size(121, 24);
            this.comDropDown.TabIndex = 0;
            this.comDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comDropDown.Items.AddRange(SerialPort.GetPortNames());
            try {
                this.comDropDown.SelectedIndex = 0;
            }
            catch (Exception) { }
            // 
            // ChoseComPornDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 148);
            this.Controls.Add(this.comDropDown);
            this.Name = "ChoseComPornDialog";
            this.Text = "ChoseComPornDialog";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
