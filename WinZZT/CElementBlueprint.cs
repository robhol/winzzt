/*

CElementBlueprint.cs

Describes the type and color of an element.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{

    class CElementBlueprint
    {

        public string Type;
        public Color Color;

        public CElementBlueprint(string str, Color defaultColor)
        {   
            
            //Get an element string describing color and type.
            //If no color, use the default one supplied.

            if (str.Length < 3)
                return;

            if (str.IndexOf("-") != -1)
            {
                string[] a = str.Split("-".ToCharArray());

                Color c;

                if (CUtil.getColorFromString(a[0], out c))
                    Color = c;
                else
                    Color = defaultColor;

                Type = a[1];
            }
            else
            {
                Type = str;
                Color = defaultColor;
            }

        }

        public bool Match(CElement e)
        {
            return (e.Type == this.Type && (e.ForeColor == this.Color || this.Color == Color.Transparent));
        }


    }

}
