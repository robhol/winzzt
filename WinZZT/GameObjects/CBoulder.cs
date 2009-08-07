using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CBoulder : CElement
    {

        public CBoulder(int x, int y, Color c)
        {

            this.ForeColor = c;
            this.BackColor = Color.Transparent;
            this.Char = 254;
            this.Block = true;
            this.Pushable = true;

            this.InitPosition(x, y);

        }

        public override void Touch()
        {
            
        }

    }
}
