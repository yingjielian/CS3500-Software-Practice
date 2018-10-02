using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 
/// Class: CS-3500
/// Author: Yingjie Lian & Xiaochuang Huang
/// Team: KungfuPanda 
/// Version: 4.2.2018
/// </summary>
namespace SpreadsheetGUI
{
    public interface ISpreadsheetView
    {
        event Action NewEvent;

        event Action CloseEvent;

        event Action<string> FileChosenEvent;

        event Action HelpEvent;

        event Action<string> SelectionChanged;

        event Action<string, string> ValueEvent;

        event Action<Stream> SaveEvent;

        void OpenNew();

        void DoClose();

        void DoHelp();

        void DoValue();

        void DoConvertToNewValue(string name, string value);

        string Message { set; }
    }
}
