using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
/// <summary>
/// 
/// Class: CS-3500
/// Author: Yingjie Lian & Xiaochuang Huang
/// Team: KungfuPanda 
/// Version: 4.2.2018
/// </summary>
namespace SpreadsheetGUI
{

    public partial class Form1 : Form, ISpreadsheetView
    {
        /// <summary>
        /// Shows a user-friendly message in the GUI.
        /// </summary>
        public string Message
        {
            set { MessageBox.Show(value); }
        }

        // Initialize the fields.
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Constructor 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            spreadsheet = new Spreadsheet();

        }


        public event Action<string> SelectionChanged;

        /// <summary>
        /// Fired when a new action is requested.
        /// </summary>
        public event Action NewEvent;

        /// <summary>
        /// Fired when a close action is requested.
        /// </summary>
        public event Action CloseEvent;

        /// <summary>
        /// Fired when a file is chosen with a file dialog.
        /// The parameter is the chosen filename.
        /// </summary>
        public event Action<string> FileChosenEvent;

        public event Action HelpEvent;

        public event Action<string, string> ValueEvent;

        public event Action ClickedEvent;

        public event Action<Stream> SaveEvent;


        /// <summary>
        /// Hold a new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            NewEvent();

        }

        /// <summary>
        /// Opens a new Spreadsheet window.
        /// </summary>
        public void OpenNew()
        {
            Tracker.GetContext().RunNew();
        }



        /// <summary>
        /// Close an event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CloseEvent();

        }

        /// <summary>
        /// Closes this window
        /// </summary>
        public void DoClose()
        {
            Close();
        }



        /// <summary>
        /// This method is going to create a GUI for the spreadsheet by a grid of 
        /// 26 columns and 99 rows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel1_KeyDown(object sender, KeyEventArgs e)
        {
            int col;
            int row;
            string value;
            string name;
            spreadsheetPanel1.GetSelection(out col, out row);

            if (e.KeyCode == Keys.Right)
            {
                col += 1;
                spreadsheetPanel1.SetSelection(col, row);
                e.Handled = true;

                spreadsheetPanel1.GetValue(col, row, out value);
                textBox2.Text = value;

                List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string new_col = AlphaList[col];
                row += 1;
                name = string.Concat(new_col.ToString(), row.ToString());

                textBox3.Text = name;

            }

            if (e.KeyCode == Keys.Left)
            {
                col -= 1;

                spreadsheetPanel1.SetSelection(col, row);

                //  SelectionChanged("A5");

                e.Handled = true;

                spreadsheetPanel1.GetValue(col, row, out value);

                textBox2.Text = value;

                List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string new_col = AlphaList[col];
                row += 1;
                name = string.Concat(new_col.ToString(), row.ToString());

                textBox3.Text = name;
            }

            if (e.KeyCode == Keys.Up)
            {
                row -= 1;

                spreadsheetPanel1.SetSelection(col, row);

                e.Handled = true;
                spreadsheetPanel1.GetValue(col, row, out value);

                textBox2.Text = value;

                List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string new_col = AlphaList[col];

                row += 1;
                name = string.Concat(new_col.ToString(), row.ToString());
                textBox3.Text = name;
            }

            if (e.KeyCode == Keys.Down)
            {
                row += 1;

                spreadsheetPanel1.SetSelection(col, row);

                e.Handled = true;

                spreadsheetPanel1.GetValue(col, row, out value);

                textBox2.Text = value;

                List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string new_col = AlphaList[col];
                row += 1;
                name = string.Concat(new_col.ToString(), row.ToString());

                textBox3.Text = name;
            }
        }


        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HelpEvent != null)
            {
                HelpEvent();
            }
        }

        public void DoHelp()
        {
            MessageBox.Show("This is the guide for the Spreadsheet GUI, You type anything in the Change Value Text field!");
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileDialog.Filter = "ss files (*.ss)|*.ss|All files (*.*)|*.*";
            DialogResult result = fileDialog.ShowDialog();

            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(fileDialog.FileName);
                }
            }
        }

        public void DoOpen()
        {
            //Stream myStream = null;


        }


        /// <summary>
        /// Returns the number of words in contents.
        /// </summary>
        private int CountWords(string contents)
        {
            StringReader reader = new StringReader(contents);
            int count = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                count += Regex.Split(line, @"\s+").Length;
            }
            return count;
        }

        /// <summary>
        /// Returns the number of lines in contents.
        /// </summary>
        private int CountLines(string contents)
        {
            StringReader reader = new StringReader(contents);
            int count = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                count++;
            }
            return count;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int col;
            int row;
            string name;
            spreadsheetPanel1.GetSelection(out col, out row);
            string value = textBox1.Text;

            string new_row = AlphaList[row];
            col += 1;

            name = string.Concat(new_row.ToString(), col.ToString());

            if (ValueEvent != null)
            {
                ValueEvent(name, value);
            }
        }

        public void DoValue()
        {
            List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int col;
            int row;
            string name;
            spreadsheetPanel1.GetSelection(out col, out row);
            string value = textBox1.Text;
            spreadsheetPanel1.SetValue(col, row, value);


            textBox2.Text = value;



            string new_col = AlphaList[col];
            row += 1;
            name = string.Concat(new_col.ToString(), row.ToString());
            textBox3.Text = name;

            spreadsheet.SetContentsOfCell(name, value);

            var newValue = spreadsheet.GetCellValue(name);
            textBox2.Text = newValue.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            DialogResult result = saveFileDialog1.ShowDialog();
            saveFileDialog1.Filter = "spreadsheet files (*.ss)|*.ss|All files (*.*)|*.*";

            try
            {
                if (result == DialogResult.OK || result == DialogResult.Yes)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        TextWriter textwriter = new StreamWriter(myStream);
                        spreadsheet.Save(textwriter);
                        //SaveEvent(myStream);
                    }
                }
            }
            catch
            {
                MessageBox.Show("can't save");
            }


        }

        public void DoSave()
        {

        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void spreadsheetPanel1_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.Select();
            if (ClickedEvent != null)
            {
                //ClickedEvent();
            }
        }

        //public void DoClickChange(String s)
        //{
        //    List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        //    int col;
        //    int row;
        //    string name;
        //    spreadsheetPanel1.GetSelection(out col, out row);
        //    string value = textBox1.Text;

        //    string new_row = AlphaList[row];
        //    col += 1;

        //    name = string.Concat(new_row.ToString(), col.ToString());

        //    textBox2.Text = s;
        //    textBox3.Text = name;
        //}

        private void spreadsheetPanel1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void spreadsheetPanel1_SelectionChanged(SSGui.SpreadsheetPanel sender)
        {

            List<string> AlphaList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int col;
            int row;
            string name;
            string textbox2;
            spreadsheetPanel1.GetSelection(out col, out row);

            spreadsheetPanel1.GetValue(col, row, out textbox2);
            string new_col = AlphaList[col];
            row += 1;

            name = string.Concat(new_col.ToString(), row.ToString());

            SelectionChanged(name);

            textBox2.Text = textbox2;
            textBox3.Text = name;
            spreadsheetPanel1.Select();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void DoConvertToNewValue(string name, string value)
        {
            int col;
            int row = 0;


            List<int> AlphaList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };

            char c = name[0];
            char upper = char.ToUpper(c);
            col = char.ToUpper(upper) - 65;

            if (name.Length == 2)
            {
                row = ((int)name[1]) - 49;
            }

            if (name.Length == 3)
            {

                row = Int32.Parse(string.Concat(name[1], name[2]));
            }

            spreadsheetPanel1.SetValue(col, row, value);


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (true)
            {
                DialogResult result = MessageBox.Show("Your spreadsheet has unsaved changes. Would you like to " +
                    "save your changes before closing?", "Save Before Exiting?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (spreadsheet.Changed)
                        e.Cancel = true;
                }
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
    }
}