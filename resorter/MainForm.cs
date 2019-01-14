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
        public MainForm() {
            ChoseComPornDialog getPort = new ChoseComPornDialog();
            getPort.ShowDialog();
            InitializeComponent();
        }
        
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent() {
            this.components = new Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Text = "resorter";
        }

    }
}
