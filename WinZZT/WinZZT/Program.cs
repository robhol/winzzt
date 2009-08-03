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
        /// 

        static public frmMain MainForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            CGrid.Initialize();
            CDrawing.Initialize();
            
            MainForm = new frmMain();
            Application.Run(MainForm);
        }
    }
}
