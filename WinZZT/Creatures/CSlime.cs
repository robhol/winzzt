/*

CSlime.cs

Expands and then turns into breakable.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CSlime : CTimedElement
    {

        public CSlime(int x, int y, Color c)
        {

            this.InitProps(x, y, 42, c, Color.Transparent, true, 61, 500);

        }

        public override void Step()
        {

            for (int di = 0; di < 4; di++)
            {
                EDirection d = (EDirection)di;
                Point p = CGrid.GetInDirection(Location,d);

                if (CGrid.IsValid(p) && !CGrid.Get(p).IsBlocked())
                {
                    new CSlime(p.X, p.Y, this.ForeColor);
                }

            }

            new CBreakable(Location.X, Location.Y, this.ForeColor);
            Die();

        }

    }
}
