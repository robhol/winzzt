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
            this.BackColor = Color.Black;
            this.Char = new string[] { "↔", "↕" }[(int)t];
            this.Block = true;
            this.Pushable = true;
            this.Orientation = t;

            this.InitPosition(x, y);

        }

        public override bool PushTowards(EDirection d)
        {

            if (Orientation == ESliderType.Horizontal && (d == EDirection.North || d == EDirection.South))
                return false;

            if (Orientation == ESliderType.Vertical && (d == EDirection.East || d == EDirection.West))
                return false;
            
            return base.PushTowards(d);
        }

        public override void Touch()
        {
            
        }

    }
}
