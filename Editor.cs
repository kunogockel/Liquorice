// Editor.cs
// Applies some additional functions around the standard TextBox control
// 2023-01-12 KG Created
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;

namespace Liquorice
{
    /// <summary>
    /// Extends the TextBox control by additional functions
    /// </summary>
    public class Editor : TextBox
    {
        /// <summary>
        /// default file name 
        /// </summary>
        const string DEFAULT_FILENAME = "untitled.txt";

        /// <summary>
        /// name of the currently loaded file
        /// </summary>
        public FileInfo m_filename = null;

        /// path of the currently edited file
        /// </summary>
        //public string m_directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// indicates, if the file has been changed
        /// </summary>
        public bool m_changed = false;

        /// <summary>
        /// last entered key char
        /// </summary>
        public char m_lastKeyChar = '\0';

        /// <summary>
        /// the text used for autoindent.
        /// This are the space characters collected from the beginning
        /// of the current line, when ENTER is pressed. They are inserted
        /// when ENTER is released.
        /// </summary>
        public string m_indent = "";

        /// <summary>
        /// Raised, when the text has been changed for the first time
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Raised, when the text has been saved
        /// </summary>
        public event EventHandler Saved;

        /// <summary>
        /// Raised, when the loaded file has changed
        /// </summary>
        public event EventHandler FileChanged;

        /// <summary>
        /// Default contructor
        /// </summary>
        public Editor()
        {
        }

        /// <summary>
        /// Handles the shortcuts
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // key is handled by default
            e.Handled = true;

            // if CTRL key is pressed
            if (e.Control)
            {
                // CTRL+N: new file
                if (e.KeyCode == Keys.N)
                    newFile();
                // CTRL+O: open file
                else if (e.KeyCode == Keys.O) 
                    openFile();
                // CTRL+R: remove tab
                else if (e.KeyCode == Keys.R)
                    removeTab();
                // CTRL+S: save file
                else if (e.KeyCode == Keys.S)
                    saveFile();
                // CTRL+T: insert tab
                else if (e.KeyCode == Keys.T)
                    insertTab();
                // CTRL+W: save file as
                else if (e.KeyCode == Keys.W)
                    saveFileAs();
                // pass key to the control
                else
                    e.Handled = false;
            }
            else
            {
                // F1: Help
                if (e.KeyCode == Keys.F1)
                {
                    MainDlg.Self.toggleHelp();
                }
                // ENTER: get indent of current line
                else if (e.KeyCode == Keys.Enter)
                {
                    // get the current line's indent
                    int indent = getIndentOfPreviousLine();
                    // if the indent has changed
                    if (indent != m_indent.Length)
                    {
                        // create an indent string with spaces
                        if (indent == 0)
                            m_indent = "";
                        else
                            m_indent = new string(' ', indent);
                    }
                }
                else
                {
                    // pass key to the control
                    e.Handled = false;
                }
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Acts on key up event
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            // ENTER: we have moved to a new line: indent this line
            // like the previous one
            if (e.KeyCode == Keys.Enter)
            {
                SelectedText = m_indent;
            }
            base.OnKeyUp(e);
        }

        /// <summary>
        /// Acts on special keys
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // key is handled by default
            e.Handled = true;

            // auto complete ()
            if (e.KeyChar == '(')
            {
                SelectedText = "()";
                SelectionStart--;
            }
            // auto complete {}
            else if (e.KeyChar == '{')
            {
                SelectedText = "{}";
                SelectionStart--;
            }
            // auto complete []
            else if (e.KeyChar == '[')
            {
                SelectedText = "[]";
                SelectionStart--;
            }
            // auto complete ''
            else if (e.KeyChar == '\'')
            {
                SelectedText = "\'\'";
                SelectionStart--;
            }
            // auto complete ""
            else if (e.KeyChar == '\"')
            {
                SelectedText = "\"\"";
                SelectionStart--;
            }
            // auto complete backtick
            else if (e.KeyChar == '`')
            {
                SelectedText = "``";
                SelectionStart--;
            }
            // auto complete /**/
            else if (e.KeyChar == '*' && m_lastKeyChar == '/')
            {
                SelectedText = "*  */";
                SelectionStart -= 3;
            }
            // auto complete <tag></tag>
            else if (e.KeyChar == '>' && m_lastKeyChar != ' ')
            {
                // if we can complete the tag, the key is handled
                e.Handled = completeHtmlTag();
            }
            else
            {
                // pass key to the control
                e.Handled = false;
            }

            // remember this key char
            m_lastKeyChar = e.KeyChar;

            // process the key
            base.OnKeyPress(e);
        }

        /// <summary>
        /// Remembers, taht the text has changed and raises the 'Changed' event.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            // only, if the flag was not set yet
            if (m_changed)
            {
                base.OnTextChanged(e);
                return;
            }
            // set changed flag
            m_changed = MainDlg.Self.TextChanged = true;
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Initializes the editor for creating a new file
        /// </summary>
        public void newFile()
        {
            // confirm to discard changes
            DialogResult dialogResult = saveUnsavedChanges();
            if (dialogResult == DialogResult.No)
            {
                saveFile();
            }
            // set initial filename
            m_filename = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + DEFAULT_FILENAME);
            // clear editor
            Text = "";
            // reset change flag
            m_changed = MainDlg.Self.TextChanged = false;
            // update filename in the main dialog
            MainDlg.Self.FileName = m_filename.Name;
        }

        /// <summary>
        /// Opens a file for editing
        /// </summary>
        public void openFile(string filename = "")
        {
            // confirm to discard changes
            DialogResult dialogResult = saveUnsavedChanges();
            if (dialogResult == DialogResult.No)
            {
                saveFile();
            }
            // if the passed filename is empty, let the user select a file
            if (filename.Length == 0)
            {
                // let the user select a file
                OpenFileDialog dlg = new OpenFileDialog()
                {
                    FileName = "",
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    Title = "Open text file"
                };
                DialogResult result = dlg.ShowDialog();
                // the dialog only allows to select an existing file or to abort the action.
                // Return, if the user has aborted the file selection
                if (result != DialogResult.OK)
                    return;
                filename = dlg.FileName;
            }
            // clear editor
            Text = "";
            // create the FileInfo object
            m_filename = new FileInfo(filename);
            // update filename in the main dialog
            MainDlg.Self.FileName = m_filename.Name;
            // load the file. In case the file was selected from the OpenFileDialog,
            // the file anyway exists. Otherwise, we must check, if the file exists.
            // If not, we initialize the Editor with a new file.
            if (m_filename.Exists)
                Text = File.ReadAllText(m_filename.FullName);
            else
                newFile();
            // reset change flag
            m_changed = MainDlg.Self.TextChanged = false;
            // reset text selection
            SelectionStart = SelectionLength = 0;
        }

        /// <summary>
        /// Saves the currently edited file
        /// </summary>
        public void saveFile()
        {
            // not, if there are noc changes
            if (!m_changed)
                return;
            // write file
            File.WriteAllText(m_filename.FullName, Text);
            // recreate the FileInfo object to reflect current changes
            m_filename = new FileInfo(m_filename.FullName);
            // reset change flag
            m_changed = MainDlg.Self.TextChanged = false;
        }

        /// <summary>
        /// Saves the currently edited file under a different name
        /// </summary>
        public void saveFileAs()
        {
            // let the user specify the name and destination
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save File";
            dlg.Filter = "Text file (*.txt)|*.txt";
            dlg.InitialDirectory = m_filename.DirectoryName;
            dlg.FileName = m_filename.Name;
            DialogResult result = dlg.ShowDialog();
            // the dialog returns OK, when the user has confirmed saving
            if (result != DialogResult.OK)
            {
                return;
            }
            // write file
            File.WriteAllText(m_filename.FullName, Text);
            // recreate the FileInfo object to reflect current changes
            m_filename = new FileInfo(m_filename.FullName);
            // reset change flag
            m_changed = MainDlg.Self.TextChanged = false;
            // update filename in the main dialog
            MainDlg.Self.FileName = m_filename.Name;
        }

        /// <summary>
        /// Asks the user to confirm discarding unsaved changes.
        /// If there are changes in the document, the user is asked, if he wants
        /// to discard them. If there are no changes, the user is not asked and
        /// the function return Yes.
        /// </summary>
        /// <returns>
        /// DialogResult.Yes - changes shall be discarded
        /// DialogResult.No - changes shall be saved
        /// </returns>
        public DialogResult saveUnsavedChanges()
        {
            if (m_changed)
                return MessageBox.Show("You have unsaved changes. Do you want to save?", Program.Version, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return DialogResult.No;
        }

        /// <summary>
        /// Completes an html tag.
        /// </summary>
        /// <returns>
        /// true, if an html tag was recognizes, otherwise false
        /// </returns>
        public bool completeHtmlTag()
        {
            // the user has just entered '>'. Look back to a
            // beginning < on the same line

            // get line index
            int lineIndex = GetLineFromCharIndex(SelectionStart);

            // get x position in line
            int x = SelectionStart - GetFirstCharIndexFromLine(lineIndex);

            // get current line
            string line = Lines[lineIndex];


            // find '<'
            int i = x - 1;
            for (; i > 0; i--)
            {
                // if we find a '/' before a '<' it must be a end-tag
                if (line[i] == '/')
                    return false;
                // if we find a '<' we are at the beginning of the tag
                if (line[i] == '<')
                    break;
            }
            if (i < 0)
            {
                // no '<' found, cannot be recognzed as an html tag
                return false;
            }

            // start of html tag found. The string from the current
            // position + 1 to x is the name of the tag
            string tagname = line.Substring(i + 1, x - i - 1);

            // append the closing tag
            SelectedText = "></" + tagname + ">";
            SelectionStart -= tagname.Length + 3;

            // tag completed
            return true;
        }

        /// <summary>
        /// Gets the indent level of the current line as count of spaces
        /// </summary>
        public int getIndentOfPreviousLine()
        {
            int x = GetFirstCharIndexOfCurrentLine();
            int y = GetLineFromCharIndex(x);
            // not, if we are in the first line
            if (y == 0)
                return 0;
            // previous line
            //y--;
            // get the previous line
            string line = Lines[y];
            // length of the line
            int length = line.Length;
            // if the length is 0, there is no indent
            if (line.Length == 0)
            {
                return 0;
            }
            int i = 0;
            for (; i < length; i++)
            {
                if (line[i] != ' ')
                    break;
            }
            // i points to the first non-space character in the line, so
            // the count of spaces is equal to i
            return i;
        }

        /// <summary>
        /// Inserts up to 4 spaces to the next tab position
        /// </summary>
        public void insertTab()
        {
            // get current char index in the line
            int x = GetFirstCharIndexOfCurrentLine();
            x = SelectionStart - x;
            // if we are at a definite tab position, insert 4 spaces to the next tab position
            if (x % 4 == 0)
            {
                SelectedText = "    ";
                return;
            }
            // we can calculate the needed spaces to the next tab position by:
            // add 4 to the current position
            x += 4;
            // get the modulo 4 of this position
            x = x % 4;
            // this is the amount of spaces to insert
            SelectedText = new string(' ', 4 - x);
        }

        /// <summary>
        /// Removes up to 4 spaces to the previous tab position
        /// </summary>
        public void removeTab()
        {
            // tab position to move back to
            int x1 = 0;
            // get current char index in the line
            int x0 = GetFirstCharIndexOfCurrentLine();
            int x = SelectionStart - x0;
            // if we are at the start of the line, there is no previous tab
            if (x == 0)
                return;
            // if we are at a definite tab position, insert 4 spaces to the next tab position
            if (x % 4 == 0)
            {
                x1 = x - 4;
            }
            else
            {
                // calulate the previous tab position
                x1 = x - (x % 4);
            }
            // select the spaces
            SelectionStart = x0 + x1;
            SelectionLength = x - x1;
            // all of these characters must be spaces
            string s = SelectedText;
            if (s.Trim().Length != 0)
            {
                // restore position
                SelectionStart = x0 + x;
                SelectionLength = 0;
                return;
            }
            // replace selected text by an empty string
            SelectedText = "";
        }
    }
}
