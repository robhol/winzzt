/*

CElement.cs

Water. Natural habitat of Jaws.
Blocks most things apart from sharks and bullets.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CWater : CElement
    {
        public CWater(int x, int y)
        {
            InitProps(x, y, 176, Color.DarkBlue, Color.Black, true, 0);
            this.BlockBullets = false;
        }
    }
}
