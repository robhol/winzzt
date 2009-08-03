using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CBreakable : CElement
    {
        public CBreakable(int x, int y, Color c)
        {
            InitProps(x, y, "▒", c, Color.Black, true, 0);
        }

        public override void Shot(CElement responsible)
        {
            this.Die();
        }

    }
}
