using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 
/// Class: CS-3500
/// Author: Yingjie Lian & Xiaochuang Huang
/// Team: KungfuPanda 
/// Version: 4.2.2018
/// </summary>
namespace SpreadsheetGUI
{
    class SpreadsheetGUIViewStub : ISpreadsheetView
    {
        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }

        public bool CalledDoHelp
        {
            get; private set;
        }

        public bool CalledDoValue
        {
            get; private set;
        }

        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        public void FireHelpEvent()
        {
            if (HelpEvent != null)
            {
                HelpEvent();
            }
        }

        public void FireValueEvent()
        {
            if (ValueEvent != null)
            {
                ValueEvent("A1", "test");
            }
        }

        public string Message { set => throw new NotImplementedException(); }

        public event Action NewEvent;
        public event Action CloseEvent;
        public event Action<string> FileChosenEvent;
        public event Action HelpEvent;
        public event Action<string> SelectionChanged;
        public event Action<string, string> ValueEvent;
        public event Action<Stream> SaveEvent;

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void DoHelp()
        {
            CalledDoHelp = true;
        }

        public void DoValue()
        {
            CalledDoValue = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void DoConvertToNewValue(string name, string value)
        {
            throw new NotImplementedException();
        }
    }
}
