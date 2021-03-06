﻿/*

CSlider.cs

Can be pushed, but only along one axis.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CSlider : CElement
    {

        ESliderType Orientation;

        public CSlider(int x, int y, ESliderType t, Color c)
        {

            this.ForeColor = c;
            this.BackColor = Color.Transparent;
            this.Char = new int[] { 29, 18 }[(int)t]; //Select appropriate chars.
            this.Block = true;
            this.Pushable = true;
            this.Orientation = t;

            this.Initialize(x, y);

        }

        public override bool PushTowards(EDirection d)
        {   
            //If trying to push in the wrong direction, refuse and return false.

            if (Orientation == ESliderType.Horizontal && (d == EDirection.North || d == EDirection.South))
                return false;

            if (Orientation == ESliderType.Vertical && (d == EDirection.East || d == EDirection.West))
                return false;

            //Otherwise, go ahead and try.
            return base.PushTowards(d);
        }

        public override void Touch(CElement responsible)
        {
            
        }

    }
}
