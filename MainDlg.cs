using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Liquorice
{
    /// <summary>
    /// Implements the main dialog window
    /// </summary>
    public partial class MainDlg : Form
    {
        string m_helpText = @"
Liquorice Help - Press [F1] to exit.

File handling
-------------
^N  New file
^O  Open a file
^S  Save currently loaded file
^W  Write out current text to a different file

Editing
-------
^X  Cut
^C  Copy
^V  Paste
^A  Select all
";

        /// <summary>
        /// gloabl reference to this dialog
        /// </summary>
        public static MainDlg Self = null;

        /// <summary>
        /// Sets the filename
        /// </summary>
        public string FileName
        {
            set { lblFileName.Text = value; }
        }

        /// <summary>
        /// indicates, if the text has been changed
        /// </summary>
        public bool TextChanged
        {
            set { lblFileName.ForeColor = value ? Color.IndianRed : Color.DarkGray; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainDlg()
        {
            InitializeComponent();
            // safe a reference to this instance
            Self = this;
        }

        /// <summary>
        /// Initializes the title bar
        /// </summary>
        private void MainDlg_Shown(object sender, EventArgs e)
        {
            // get filename from passed commandline argument
            if (Program.Args.Length > 0)
            {
                // only, if the file exists
                FileInfo fi = new FileInfo(Program.Args[0]);
                if (fi.Exists)
                {
                    txtEditor.openFile(fi.FullName);
                }
                else
                {
                    MessageBox.Show("File not found: " + fi.FullName, Program.Version, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Process key strokes on form level
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool result = true;
            switch (keyData)
            {
                case Keys.F1:
                    toggleHelp();
                    break;
                default:
                    result = false;
                    break;
            }
            if (result)
                return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Toggles the help screen
        /// </summary>
        public void toggleHelp()
        {
            if (txtHelp.Visible)
            {
                txtHelp.Visible = false;
                txtEditor.Visible = true;
            }
            else
            {
                if (txtHelp.Text.Length == 0)
                {
                    txtHelp.Text = m_helpText;
                    txtHelp.SelectionStart = 0;
                    txtHelp.SelectionLength = 0;
                }
                txtHelp.Visible = true;
                txtEditor.Visible = false;
            }
        }
    }
}
