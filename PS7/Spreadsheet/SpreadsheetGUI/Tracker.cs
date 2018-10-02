using System;
using System.Collections.Generic;
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
    class Tracker : ApplicationContext
    {

        // Number of open forms
        private int windowCount = 0;

        // Singleton ApplicationContext
        private static Tracker context;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private Tracker()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static Tracker GetContext()
        {
            if (context == null)
            {
                context = new Tracker();
            }
            return context;
        }

        /// <summary>
        /// Runs a form in this application context
        /// </summary>
        public void RunNew()
        {
            // Create the window
            Form1 window = new Form1();
            new Controller(window);

            // One more form is running
            windowCount++;

            // When this form closes, we want to find out
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();
        }

        public Form1 Open()
        {
            // Create the window
            Form1 window = new Form1();
            new Controller(window);

            // One more form is running
            windowCount++;

            // When this form closes, we want to find out
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();
            return window;
        }


    }
}
