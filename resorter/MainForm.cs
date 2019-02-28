using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resorter {
    // main UI thread
    public class MainForm : Form {
        // variable for storing the state handler
        ResorterStateHandler stateHandler;
        // constructor
        public MainForm() {
            // create the dialog box for the user to chose the COM port
            ChoseComPortDialog getPort = new ChoseComPortDialog();
            // open the dialog (blocks thread)
            getPort.ShowDialog();

            // try and create the state handler
            try {
                stateHandler = new ResorterStateHandler(getPort.comDropDown.Text, Program.Settings.Steps);
            }
            // oof, didnt work
            catch (ArgumentException) {
                MessageBox.Show("couldnt connect to COM port");
                Close();
            }
            //create all the UI components
            InitializeComponent();
            // when the settings button is clicked
            settingsButton.Click += (object sender, EventArgs e) => {
                // create the settings form
                SettingsForm settingsForm = new SettingsForm();
                Console.WriteLine("opening settings dialog");
                // open the form (blocks thread)
                settingsForm.ShowDialog();
                // opdate the variables 
                Program.UpdateSettingsFile();
                Console.WriteLine("updated settings");
            };
            // when the calibrations button is clicked
            calibrateButton.Click += (object sender, EventArgs e) => {
                // create the calibration form
                CalibrationForm calibrationForm = new CalibrationForm(stateHandler.ComHandler);
                Console.WriteLine("openning calibration dialog");
                // open the form (blocks thread)
                calibrationForm.ShowDialog();
                Console.WriteLine("closing calibration dialog");
            };
            // when this formed is loaded up and ready
            Load += (object sender, EventArgs e) => {
                // create the writer and writes everything from Console.Log to a UI textbox
                TextBoxStreamWriter textWriter = new TextBoxStreamWriter(txtConsole);
                // create a stream to a file with the date and time as name
                FileStream logFile = File.Create("log_" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".txt");
                // when we get a now log message
                textWriter.OnLog += (object _, char c) => {
                    // write to the file
                    byte[] data = Encoding.UTF8.GetBytes(new char[] { c });
                    logFile.Write(data, 0, data.Length);
                };
                // set the console to log to the writer
                Console.SetOut(textWriter);
            };
            // when the start button is clicked
            startButton.Click += (object sender, EventArgs e) => {
                // start the statehandler
                stateHandler.Start();
            };
            // when the stop button is clicked
            stopButton.Click += async (object sender, EventArgs e) => {
                // wait the state handler to stop
                List<float>[] list = await stateHandler.StopWhenPossible();
                // create a variable to make the string for the output file
                StringBuilder str = new StringBuilder();
                // for each chamber
                for (int i = 0; i < list.Length; i++) {
                    // the chamber with number of chamber
                    str.Append("chamber " + i);
                    // for each entry in each chamber
                    list[i].ForEach(x => {
                        // add tab
                        str.Append("\t");
                        // add the resistors ohmsk resistance
                        str.Append(x);
                    });
                }
                // file name for the output file
                string filename = Path.Combine("run_" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".txt");
                // create the file;
                FileStream file = File.Create(filename);
                // get the string as bytes;
                byte[] b = Encoding.ASCII.GetBytes(str.ToString());
                // write the bytes to the file
                file.Write(b, 0, b.Length);
                // close stream
                file.Close();
                // show the file in the notepad (or other .txt viewing programs the user have configures windows to use)
                Process.Start(filename);
            };
        }

        // all the UI elements
        private Button settingsButton;
        private Label statusLabel;
        private TextBox txtConsole;
        private Button calibrateButton;
        private Button startButton;
        private Button stopButton;
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // creating all the UI elements and configure them
        // this is not particularly imporant
        private void InitializeComponent() {
            this.settingsButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(713, 12);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(75, 33);
            this.settingsButton.TabIndex = 0;
            this.settingsButton.Text = "settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(13, 13);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            this.statusLabel.TabIndex = 1;
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(16, 61);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.Size = new System.Drawing.Size(347, 266);
            this.txtConsole.TabIndex = 2;
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(632, 12);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 33);
            this.calibrateButton.TabIndex = 3;
            this.calibrateButton.Text = "calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(713, 405);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 33);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "start";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(544, 405);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(149, 33);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "stop when possible ";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.calibrateButton);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.settingsButton);
            this.Name = "MainForm";
            this.Text = "resorter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
