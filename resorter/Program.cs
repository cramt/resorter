using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resorter {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            /*
            var a = "\"your,mom\",23,[2,3,4]".SplitWithin(',', new char[][] { new char[] { '\"', '\"' }, new char[] { '[', ']' } });
            a.ForEach(Console.WriteLine);
            Thread.Sleep(-1);
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
