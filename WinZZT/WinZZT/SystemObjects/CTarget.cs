using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CTarget : CTimedElement
    {

        public CTarget(int x, int y)
        {

            this.ForeColor = Color.Gray;
            this.BackColor = Color.Black;
            this.Char = "o";
            this.Cycle = 500;
            this.Block = true;

            this.InitPosition(x, y);

        }

        public override void Step()
        {

        }

        public override void Shot(CElement responsible)
        {
            Random r = new Random();
            this.ForeColor = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CDrawing.DisplayText("Target - Shot", 1000);
        }

        public override void Touch()
        {
            Random r = new Random();
            this.ForeColor = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CDrawing.DisplayText("Target - Touched", 1000);
        }

    }
}
