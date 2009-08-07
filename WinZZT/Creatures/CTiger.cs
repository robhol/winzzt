using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CTiger : CTimedElement
    {

        public CTiger(int x, int y)
        {
            this.InitProps(x, y, 227, Color.Cyan, Color.Transparent, true, 65, 700);
            
        }

        DateTime NextShot = DateTime.Now;
        double shotFreq = 1500;

        public override void Step()
        {
            Seek(CGame.Player);
            Point pLoc = CGame.Player.Location;

            if (pLoc.X == Location.X || pLoc.Y == Location.Y && NextShot < DateTime.Now)
            {
                NextShot = DateTime.Now.AddMilliseconds(shotFreq);
                Shoot(CGrid.GetDirectionToPoint(Location, pLoc, false));
            }

        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            Die();
        }

    }
}
