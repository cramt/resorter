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
    public class MainForm : Form {
        ResorterStateHandler stateHandler;
        public MainForm() {
            ChoseComPortDialog getPort = new ChoseComPortDialog();
            getPort.ShowDialog();

            try {
                stateHandler = new ResorterStateHandler(getPort.comDropDown.Text, Program.Settings.Steps);
            }
            catch (ArgumentException) {
                MessageBox.Show("couldnt connect to COM port");
                Close();
            }
            InitializeComponent();
            settingsButton.Click += (object sender, EventArgs e) => {
                SettingsForm settingsForm = new SettingsForm();
                Console.WriteLine("opening settings dialog");
                settingsForm.ShowDialog();
                Program.UpdateSettingsFile();
                Console.WriteLine("updated settings");
            };
            calibrateButton.Click += (object sender, EventArgs e) => {
                CalibrationForm calibrationForm = new CalibrationForm(stateHandler.ComHandler);
                Console.WriteLine("openning calibration dialog");
                calibrationForm.ShowDialog();
                Console.WriteLine("closing calibration dialog");
            };
            Load += (object sender, EventArgs e) => {
                TextBoxStreamWriter textWriter = new TextBoxStreamWriter(txtConsole);
                FileStream logFile = File.Create("log_" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".txt");
                textWriter.OnLog += (object _, char c) => {
                    byte[] data = Encoding.UTF8.GetBytes(new char[] { c });
                    logFile.Write(data, 0, data.Length);
                };
                Console.SetOut(textWriter);
            };
            startButton.Click += (object sender, EventArgs e) => {
                stateHandler.Start();
            };
            stopButton.Click += async (object sender, EventArgs e) => {
                List<float>[] list = await stateHandler.StopWhenPossible();
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < list.Length; i++) {
                    str.Append("chamber " + i);
                    list[i].ForEach(x => {
                        str.Append("\t");
                        str.Append(x);
                    });
                }
                
                string filename = Path.Combine("run_" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".txt");
                FileStream file = File.Create(filename);
                string s = str.ToString();
                byte[] b = Encoding.ASCII.GetBytes(s);
                file.Write(b, 0, b.Length);
                file.Close();
                Process.Start(filename);
            };
        }

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
