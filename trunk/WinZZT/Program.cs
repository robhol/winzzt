/*

Program.cs

Where the program starts.
Calls the initialization functions for some sub-systems.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinZZT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static public frmMain MainForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Initialize sub-systems
            CCharManager.Initialize();
            CElementManager.Initialize();
            CGrid.Initialize();
            CGame.Initialize();
            CDrawing.Initialize();
            
            //Create and display main form.
            MainForm = new frmMain();
            Application.Run(MainForm);
        }
    }
}
