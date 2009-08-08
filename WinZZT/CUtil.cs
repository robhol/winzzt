using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    static class CUtil
    {

        public static Color getColorFromString(string s)
        {

            int r = int.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            int a = 255;

            if (s.Length == 8) //if getting alpha
                a = int.Parse(s.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a,r,g,b);

        }

    }
}
