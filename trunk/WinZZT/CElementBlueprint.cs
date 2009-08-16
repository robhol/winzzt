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

            if (str.IndexOf("-") != -1)
            {
                string[] a = str.Split("-".ToCharArray());
                Color = CUtil.getColorFromString(a[0]);
                Type = a[1];
            }
            else
            {
                Type = str;
                Color = defaultColor;
            }

        }


    }

}
