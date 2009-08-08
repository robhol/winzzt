using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CInvisibleWall : CElement
    {
        public CInvisibleWall(int x, int y)
        {
            InitProps(x, y, 32, Color.White, Color.Black, true, 0);
        }

    }
}
