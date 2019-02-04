using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resorter {
    public class CalibrationForm : Form {
        private Button turnLeftButton;
        private Button turnRightButton;
        private Button doneButton;

        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            this.turnLeftButton = new System.Windows.Forms.Button();
            this.turnRightButton = new System.Windows.Forms.Button();
            this.doneButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // turnLeftButton
            // 
            this.turnLeftButton.Location = new System.Drawing.Point(12, 81);
            this.turnLeftButton.Name = "turnLeftButton";
            this.turnLeftButton.Size = new System.Drawing.Size(75, 23);
            this.turnLeftButton.TabIndex = 0;
            this.turnLeftButton.Text = "<";
            this.turnLeftButton.UseVisualStyleBackColor = true;
            // 
            // turnRightButton
            // 
            this.turnRightButton.Location = new System.Drawing.Point(174, 81);
            this.turnRightButton.Name = "turnRightButton";
            this.turnRightButton.Size = new System.Drawing.Size(75, 23);
            this.turnRightButton.TabIndex = 1;
            this.turnRightButton.Text = ">";
            this.turnRightButton.UseVisualStyleBackColor = true;
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(93, 81);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(75, 23);
            this.doneButton.TabIndex = 2;
            this.doneButton.Text = "done";
            this.doneButton.UseVisualStyleBackColor = true;
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.turnRightButton);
            this.Controls.Add(this.turnLeftButton);
            this.Name = "CalibrationForm";
            this.Text = "calibration form";
            this.ResumeLayout(false);

        }

        #endregion

        private JSFNComHandler handler;

        public CalibrationForm(JSFNComHandler handler) {
            this.handler = handler;
            InitializeComponent();
            int direction = 0;
            bool going = true;
            Task.Factory.StartNew(async () => {
                while (going) {
                    await handler.SendFunction("catcherTurn", new object[] { direction, 60 });
                }
            });
            turnLeftButton.MouseDown += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = 1;
                }
            };
            turnLeftButton.MouseUp += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = 0;
                }
            };
            turnRightButton.MouseDown += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = -1;
                }
            };
            turnRightButton.MouseUp += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = 0;
                }
            };
            doneButton.Click += (object sender, EventArgs e) => {
                going = false;
                this.Close();
            };
        }
    }
}
