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
    // this a popup form for calibrating the position of the catcher stepper moter, so that it is in position for the state handler
    public class CalibrationForm : Form {
        // ui stuff, kidna irrelevant
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
        // more ui stuff
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
        // the handler for communicating with the arduino in JSFN 
        // this is not throught the state handler, cause we dont want to the state handler to change its perception of the currents steps when we change them
        private JSFNComHandler handler;

        public CalibrationForm(JSFNComHandler handler) {
            //set the handler
            this.handler = handler;
            // start the ui
            InitializeComponent();
            // variable for holding current direction
            int direction = 0;
            // variable for stopping loop again
            bool going = true;
            // start another thread
            Task.Factory.StartNew(async () => {
                //just keep going
                while (going) {
                    //turn the catcher to whatever is in the direction variable (this is not positon, but direction)
                    await handler.SendFunction("catcherTurn", new object[] { direction, 60, 20 });
                }
            });
            //if the button is pressed down, set direction to 1
            turnLeftButton.MouseDown += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = 1;
                }
            };
            //if left button is released again, set it back to 0
            turnLeftButton.MouseUp += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = 0;
                }
            };
            //if right button is pressed down, set it to -1
            turnRightButton.MouseDown += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = -1;
                }
            };
            //if right button is released again, set it back to 0
            turnRightButton.MouseUp += (object sender, MouseEventArgs e) => {
                if (e.Button == MouseButtons.Left) {
                    direction = 0;
                }
            };
            //when we're done, we set going to false to stop the loop and close the whole thing
            doneButton.Click += (object sender, EventArgs e) => {
                going = false;
                this.Close();
            };
        }
    }
}
