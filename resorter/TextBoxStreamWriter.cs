using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace resorter {
    //class for redirecting the Console.WriteLine (and other Console functions)
    //we basically define a custom console
    //we are gonna make the console print to a textbox and allow other classes to hook events into this class
    public class TextBoxStreamWriter : TextWriter {
        // define the textbox to write to
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output) {
            // set the textbox based on argument
            _output = output;
        }

        public event EventHandler<char> OnLog;

        // when ever at character is written to the console
        public override void Write(char value) {
            // call the base (we gotta do this, else windows gets mad)
            base.Write(value);
            // we have to call invoke function, because you cant change the UI when not in the UI thread
            // this makes sure that the function we pass as an argument will be run on the UI thread (specificly on the textbox'es UI thread)
            _output.Invoke(new Action(() => {
                // this is what other classes can hook into, so that they also get to know when something is written to the console
                OnLog(null, value);
                // we add the character we got, to the textbox
                _output.AppendText(value.ToString());
            }));
        }
        // we define out encoding style as being UTF-8, because this is 2019 and why would ever use anything else
        public override Encoding Encoding {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
