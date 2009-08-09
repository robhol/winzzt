/*

CNormal.cs

This is really a wall. I promise.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CNormal : CElement
    {
        public CNormal(int x, int y, Color c)
        {
            InitProps(x, y, 178, c, Color.Black, true, 0);
        }
    }
}
