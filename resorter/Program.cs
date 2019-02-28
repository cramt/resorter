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
        // public global settings all of everything can read
        public static Settings Settings;
        // function to update the file based on the current value of the settings variable
        public static void UpdateSettingsFile() {
            // formatter for serialization
            BinaryFormatter bf = new BinaryFormatter();
            // new stream
            var ms = new MemoryStream();
            // serialize the variable into the stream
            bf.Serialize(ms, Settings);
            // get the data from the stream
            byte[] data = ms.ToArray();
            // if file exist, delete it
            if (File.Exists(settingsFileName)) {
                File.Delete(settingsFileName);
            }
            // create a new file with the filename
            FileStream fs = File.Create(settingsFileName);
            // write the data to the file
            fs.Write(data, 0, data.Length);
            // close stream
            fs.Close();

        }
        private const string settingsFileName = "settings.settings";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // file name of the settings file
            const string settingsFileName = "settings.settings";
            // if the file exists
            if (File.Exists(settingsFileName)) {
                // create formatter for deserialization
                BinaryFormatter bf = new BinaryFormatter();
                // open stream from file
                FileStream fs = File.OpenRead(settingsFileName);
                // deserialize some shit
                Settings = (Settings)bf.Deserialize(fs);
                // close stream
                fs.Close();
            }
            else {
                // if the file is not a thing
                // get standart settings
                Settings = Settings.StandartSettings;
                // update the settings
                UpdateSettingsFile();
            }
            
            // standart things for windows applications
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try {
                //star the UI thread
                Application.Run(new MainForm());
            }
            catch (Exception) { }
        }
    }
}
