using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resorter {
    public partial class SettingsForm : Form {

        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.mainSettingsTabControl = new System.Windows.Forms.TabControl();
            this.generalSettings = new System.Windows.Forms.TabPage();
            this.toleranceUnitDrowDown = new System.Windows.Forms.ComboBox();
            this.toleranceTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chamber3TextBox = new System.Windows.Forms.TextBox();
            this.chamber2TextBox = new System.Windows.Forms.TextBox();
            this.chamber1TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.speedSettings = new System.Windows.Forms.TabPage();
            this.mainSettingsTabControl.SuspendLayout();
            this.generalSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSettingsTabControl
            // 
            this.mainSettingsTabControl.Controls.Add(this.generalSettings);
            this.mainSettingsTabControl.Controls.Add(this.speedSettings);
            this.mainSettingsTabControl.Location = new System.Drawing.Point(5, 3);
            this.mainSettingsTabControl.Name = "mainSettingsTabControl";
            this.mainSettingsTabControl.SelectedIndex = 0;
            this.mainSettingsTabControl.Size = new System.Drawing.Size(795, 447);
            this.mainSettingsTabControl.TabIndex = 0;
            // 
            // generalSettings
            // 
            this.generalSettings.Controls.Add(this.toleranceUnitDrowDown);
            this.generalSettings.Controls.Add(this.toleranceTextBox);
            this.generalSettings.Controls.Add(this.label10);
            this.generalSettings.Controls.Add(this.label9);
            this.generalSettings.Controls.Add(this.label8);
            this.generalSettings.Controls.Add(this.label6);
            this.generalSettings.Controls.Add(this.chamber3TextBox);
            this.generalSettings.Controls.Add(this.chamber2TextBox);
            this.generalSettings.Controls.Add(this.chamber1TextBox);
            this.generalSettings.Controls.Add(this.label4);
            this.generalSettings.Controls.Add(this.label3);
            this.generalSettings.Controls.Add(this.label2);
            this.generalSettings.Controls.Add(this.label1);
            this.generalSettings.Location = new System.Drawing.Point(4, 25);
            this.generalSettings.Name = "generalSettings";
            this.generalSettings.Padding = new System.Windows.Forms.Padding(3);
            this.generalSettings.Size = new System.Drawing.Size(787, 418);
            this.generalSettings.TabIndex = 0;
            this.generalSettings.Text = "general";
            this.generalSettings.UseVisualStyleBackColor = true;
            // 
            // toleranceUnitDrowDown
            // 
            this.toleranceUnitDrowDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toleranceUnitDrowDown.FormattingEnabled = true;
            this.toleranceUnitDrowDown.Items.AddRange(new object[] {
            "Ω",
            "%"});
            this.toleranceUnitDrowDown.Location = new System.Drawing.Point(197, 130);
            this.toleranceUnitDrowDown.Name = "toleranceUnitDrowDown";
            this.toleranceUnitDrowDown.Size = new System.Drawing.Size(49, 24);
            this.toleranceUnitDrowDown.TabIndex = 6;
            // 
            // toleranceTextBox
            // 
            this.toleranceTextBox.Location = new System.Drawing.Point(90, 130);
            this.toleranceTextBox.Name = "toleranceTextBox";
            this.toleranceTextBox.Size = new System.Drawing.Size(100, 22);
            this.toleranceTextBox.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 17);
            this.label10.TabIndex = 13;
            this.label10.Text = "Tolerance";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(197, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 17);
            this.label9.TabIndex = 12;
            this.label9.Text = "Ω";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(197, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 17);
            this.label8.TabIndex = 11;
            this.label8.Text = "Ω";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Ω";
            // 
            // chamber3TextBox
            // 
            this.chamber3TextBox.Location = new System.Drawing.Point(90, 99);
            this.chamber3TextBox.Name = "chamber3TextBox";
            this.chamber3TextBox.Size = new System.Drawing.Size(100, 22);
            this.chamber3TextBox.TabIndex = 3;
            // 
            // chamber2TextBox
            // 
            this.chamber2TextBox.Location = new System.Drawing.Point(90, 68);
            this.chamber2TextBox.Name = "chamber2TextBox";
            this.chamber2TextBox.Size = new System.Drawing.Size(100, 22);
            this.chamber2TextBox.TabIndex = 2;
            // 
            // chamber1TextBox
            // 
            this.chamber1TextBox.Location = new System.Drawing.Point(90, 37);
            this.chamber1TextBox.Name = "chamber1TextBox";
            this.chamber1TextBox.Size = new System.Drawing.Size(100, 22);
            this.chamber1TextBox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Chamber 3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Chamber 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Chamber 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Champers";
            // 
            // speedSettings
            // 
            this.speedSettings.Location = new System.Drawing.Point(4, 25);
            this.speedSettings.Name = "speedSettings";
            this.speedSettings.Padding = new System.Windows.Forms.Padding(3);
            this.speedSettings.Size = new System.Drawing.Size(787, 418);
            this.speedSettings.TabIndex = 1;
            this.speedSettings.Text = "speed";
            this.speedSettings.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainSettingsTabControl);
            this.Name = "SettingsForm";
            this.Text = "settings";
            this.mainSettingsTabControl.ResumeLayout(false);
            this.generalSettings.ResumeLayout(false);
            this.generalSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        private void ApplySettings() {
            chamberTextBoxes = new TextBox[] {
                chamber1TextBox,
                chamber2TextBox,
                chamber3TextBox,
            };
            SettingsObject = Program.Settings;

            string CleanForDigits(string str) {
                Regex digitsOnly = new Regex(@"[^\d]");
                return digitsOnly.Replace(str, "");
            }

            void textBoxTextChanged(object sender, EventArgs e) {
                for (int i = 0; i < chamberTextBoxes.Length; i++) {
                    chamberTextBoxes[i].Text = CleanForDigits(chamberTextBoxes[i].Text);
                    if (int.TryParse(chamberTextBoxes[i].Text, out int result)) {
                        SettingsObject.Chambers[i] = result;
                    }
                }
            }

            for (int i = 0; i < chamberTextBoxes.Length; i++) {
                chamberTextBoxes[i].Text = SettingsObject.Chambers[i].ToString();
                chamberTextBoxes[i].TextChanged += textBoxTextChanged;
            }

            toleranceTextBox.Text = SettingsObject.Tolerance.ToString();

            toleranceTextBox.TextChanged += (object sender, EventArgs e) => {
                toleranceTextBox.Text = CleanForDigits(toleranceTextBox.Text);
                if (int.TryParse(toleranceTextBox.Text, out int result)) {
                    SettingsObject.Tolerance = result;
                }
            };

            toleranceUnitDrowDown.SelectedIndex = SettingsObject.ToleranceIsPercentage ? 1 : 0;
            toleranceUnitDrowDown.TextChanged += (object sender, EventArgs e) => {
                SettingsObject.ToleranceIsPercentage = toleranceUnitDrowDown.Text == "%";
            };
        }


        public SettingsForm() {
            InitializeComponent();
            ApplySettings();
        }

        public Settings SettingsObject { get; private set; }

        private TabControl mainSettingsTabControl;
        private TabPage generalSettings;
        private TextBox toleranceTextBox;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label6;
        private TextBox chamber3TextBox;
        private TextBox chamber2TextBox;
        private TextBox chamber1TextBox;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        public ComboBox toleranceUnitDrowDown;
        private TabPage speedSettings;
        private TextBox[] chamberTextBoxes;
    }
}
