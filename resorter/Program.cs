using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resorter {
    static class Program {
        public static Settings Settings;
        public static void UpdateSettingsFile() {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream()) {
                bf.Serialize(ms, Settings);
                byte[] data = ms.ToArray();
                if (File.Exists(settingsFileName)) {
                    File.Delete(settingsFileName);
                }
                FileStream fs = File.Create(settingsFileName);
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        private const string settingsFileName = "settings.settings";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            const string settingsFileName = "settings.settings";
            if (File.Exists(settingsFileName)) {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream()) {
                    FileStream fs = File.OpenRead(settingsFileName);
                    Settings = (Settings)bf.Deserialize(fs);
                    fs.Close();
                }
            }
            else {
                Settings = Settings.StandartSettings;
                UpdateSettingsFile();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try {
                Application.Run(new MainForm());
            }
            catch (Exception) { }
        }
    }
}
