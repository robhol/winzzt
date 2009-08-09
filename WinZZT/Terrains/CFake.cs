/*

CFake.cs

It's a wall. 
... or is it?
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CFake : CElement
    {
        public CFake(int x, int y, Color c)
        {
            InitProps(x, y, 178, c, Color.Black, false, 5);
        }
    }
}
