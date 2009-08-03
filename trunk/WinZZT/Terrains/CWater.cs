using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CWater : CElement
    {
        public CWater(int x, int y, Color c)
        {
            InitProps(x, y, "░", c, Color.Black, true, 0);
            this.BlockBullets = false;
        }
    }
}
