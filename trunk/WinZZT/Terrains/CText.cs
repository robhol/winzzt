/*

CText.cs

Wall. Again, really. Equivalent of Solid in ZZT.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CText : CElement
    {
        public CText(int x, int y, int chr, Color foreground, Color background)
        {
            InitProps(x, y, chr, foreground, background, true, 997);
        }
    }
}
