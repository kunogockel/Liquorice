// Program.cs
// Implements a minimalistic Editor around the standard TextBox control
// 2023-01-12 KG Created
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liquorice
{
    public static class Program
    {
        /// <summary>
        /// Program title and version
        /// </summary>
        public static string Version = "Liquorice 2023-01-12";

        /// <summary>
        /// Commandline arguments
        /// </summary>
        public static string[] Args = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Args = args;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainDlg());
        }
    }
}
