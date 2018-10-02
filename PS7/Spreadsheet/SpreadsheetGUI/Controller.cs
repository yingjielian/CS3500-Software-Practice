using SS;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// Class: CS-3500
/// Author: Yingjie Lian & Xiaochuang Huang
/// Team: KungfuPanda 
/// Version: 4.2.2018
/// </summary>
namespace SpreadsheetGUI
{
    public class Controller
    {
        // The window being controlled
        private ISpreadsheetView window;

        public Spreadsheet spreadsheet;


        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(ISpreadsheetView window)
        {
            this.window = window;
            this.spreadsheet = new Spreadsheet();
            window.NewEvent += HandleNew;
            window.CloseEvent += HandleClose;
            window.FileChosenEvent += HandleFileChosen;
            window.SelectionChanged += ChangeSelection;
            window.HelpEvent += HandleHelp;
            window.ValueEvent += HandleValue;
            window.SaveEvent += HandleSave;
        }


        private void HandleNew()
        {
            window.OpenNew();
        }

        private void HandleClose()
        {
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void HandleFileChosen(String filename)
        {
            Spreadsheet spreadsheet_2;
            Regex new_regex = new Regex(@"^[a-zA-z]+[1-9][0-9]*$");

            try
            {
                using (TextReader read = new StreamReader(filename))
                {
                    spreadsheet_2 = new Spreadsheet(read, new_regex);

                    Dictionary<string, string> total_value = new Dictionary<string, string>();

                    foreach (string i in spreadsheet_2.GetNamesOfAllNonemptyCells())
                    {
                        total_value.Add(i, spreadsheet_2.GetCellValue(i).ToString());
                    }

                    ISpreadsheetView new_form1 = Tracker.GetContext().Open();

                    Controller new_controller = new Controller(new_form1);

                    new_controller.spreadsheet = spreadsheet_2;

                    foreach (string i in new_controller.spreadsheet.GetNamesOfAllNonemptyCells())
                    {
                        new_controller.HandleNewValue(i, new_controller.spreadsheet.GetCellValue(i).ToString());
                    }
                }
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        private void ChangeSelection(string s)
        {
            string k = spreadsheet.GetCellValue(s).ToString();
        }

        private void HandleHelp()
        {
            window.DoHelp();
        }

        private void HandleValue(string name, string value)
        {
            try
            {
                window.DoValue();
            }
            catch (Exception ex)
            {
                window.Message = "Invalid Value!" + ex.Message;
            }
        }

        public void HandleSave(Stream mystream)
        {
            TextWriter textwriter = new StreamWriter(mystream);
            spreadsheet.Save(textwriter);
        }

        public void HandleNewValue(string name, string value)
        {
            window.DoConvertToNewValue(name, value);
        }

    }
}
