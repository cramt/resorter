using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace resorter {
    public class TextBoxStreamWriter : TextWriter {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output) {
            _output = output;
        }

        public event EventHandler<char> OnLog;

        public override void Write(char value) {
            base.Write(value);
            _output.Invoke(new Action(() => {
                OnLog(null, value);
                _output.AppendText(value.ToString());
            }));
        }

        public override Encoding Encoding {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
