/*

CUtil.cs

Functions that would be useful, but not fit too well at other places.
  
*/

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

        /// <summary>
        /// Parses a 6-/8-digit color string in the RRGGBB or RRGGBBAA format.
        /// </summary>
        /// <param name="s">The color string.</param>
        /// <returns></returns>
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
