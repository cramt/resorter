using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        JSFNComHandler jsfnHandler;
        public MainForm() {
            /*
            ChoseComPortDialog getPort = new ChoseComPortDialog();
            getPort.ShowDialog();
            Dictionary<string, Func<List<object>, object>> jsfnFuncs = new Dictionary<string, Func<List<object>, object>>();
            jsfnFuncs.Add("print", new Func<List<object>, object>((List<object> l) => {
                Console.WriteLine(string.Join(",", l.ToArray()));
                return null;
            }));
            jsfnHandler = new JSFNComHandler(getPort.comDropDown.Text, jsfnFuncs);
            */
            InitializeComponent();
            settingsButton.Click += (object sender, EventArgs e) => {
                SettingsForm settingsForm = new SettingsForm();
                settingsForm.ShowDialog();
                Program.UpdateSettingsFile();
            };
            Load += (object sender, EventArgs e) => {
                TextBoxStreamWriter textWriter = new TextBoxStreamWriter(txtConsole);
                FileStream logFile = File.Create("log "+ DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")  + ".txt");
                textWriter.OnLog += (object _, char c) => {
                    byte[] data = Encoding.UTF8.GetBytes(new char[] { c });
                    logFile.Write(data, 0, data.Length);
                };
                Console.SetOut(textWriter);
            };
        }

        private Button settingsButton;
        private Label label1;
        private TextBox txtConsole;
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent() {
            this.settingsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConsole = new System.Windows.Forms.TextBox();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(16, 61);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(347, 266);
            this.txtConsole.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.settingsButton);
            this.Name = "MainForm";
            this.Text = "resorter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
