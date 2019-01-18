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
    public class MainForm : Form {
        JSFNComHandler jsfnHandler;
        public MainForm() {
            ChoseComPornDialog getPort = new ChoseComPornDialog();
            getPort.ShowDialog();
            Dictionary<string, Func<List<object>, object>> jsfnFuncs = new Dictionary<string, Func<List<object>, object>>();
            jsfnFuncs.Add("print", new Func<List<object>, object>((List<object> l) => {
                Console.WriteLine(l[0]);
                return null;
            }));
            jsfnHandler = new JSFNComHandler(getPort.comDropDown.Text, jsfnFuncs);
            InitializeComponent();
        }

        private Button button1;
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "resorter";
            this.ResumeLayout(false);

        }

        private void button1_Click(object sender, EventArgs e) {
            jsfnHandler.SendFunction("add", new object[] { 2 }).ContinueWith(x => {
                Console.WriteLine(x.Result[0]);
            });
        }
    }
}
