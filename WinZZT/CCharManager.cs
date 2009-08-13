/*

CCharManager.cs

Deals with fetching chars out of the bitmap resource.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    static class CCharManager
    {

        private static Bitmap[] bChars = new Bitmap[256];

        private static Bitmap CopyRegion(Bitmap src, Rectangle r)
        {

            Bitmap b = new Bitmap(r.Width, r.Height);
            
            Graphics g = Graphics.FromImage(b);

            g.DrawImage(src, 0, 0, r, GraphicsUnit.Pixel);

            g.Dispose();

            return b;

        }

        public static void Initialize()
        {

            for (int i = 0; i < 256; i++)
            {
                bChars[i] = CopyRegion(Properties.Resources.charmap, new Rectangle(i * 8 + 1, 0, 8, 14));
            }
            
        }

        public static Bitmap GetChar(int chr, Color c)
        {

            //Bitmap b = CopyRegion(Properties.Resources.charmap, new Rectangle(chr * 8 + 1, 0, 8, 14));
            Bitmap b = (Bitmap)bChars[chr].Clone();

            for (int cy = 0; cy < 14; cy++)
                for (int cx = 0; cx < 8; cx++)
                {
                    if (b.GetPixel(cx, cy) == Color.FromArgb(0, 0, 0))
                        b.SetPixel(cx, cy, c);
                    else
                        b.SetPixel(cx, cy, Color.Transparent);
                }

            return b;
        }




    }
}
