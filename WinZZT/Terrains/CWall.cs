/*

CWall.cs

Wall. Again, really. Equivalent of Solid in ZZT.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CWall : CElement
    {
        public CWall(int x, int y, Color c)
        {
            InitProps(x, y, 219, c, Color.Black, true, 0);
        }
    }
}
