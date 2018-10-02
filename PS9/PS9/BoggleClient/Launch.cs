// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    /// <summary>
    /// Contains the main method that launches this application. 
    /// The player selection window is what's opened by default.
    /// </summary>
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             
            MatchmakingForm window = new MatchmakingForm();
            MatchmakerController controller = new MatchmakerController(window);
            Application.Run(window);
        }
    }
}
