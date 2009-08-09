/*

CBullet.cs

Bullets... come from somewhere, go somewhere.
When they hit something, they also fire Shot() on it.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CBullet : CTimedElement
    {
        public EDirection Direction;
        public CElement Source;

        public CBullet(int x, int y, int cCycle, EDirection direction, CElement source)
        {
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.Char = 249;
            this.Block = true;

            this.Source = source;

            this.Ordering = 999;

            Direction = direction;
            this.Cycle = cCycle;
            this.Location = new Point(x, y);
            this.Initialize(x, y);
            this.InitTimer(cCycle);
        }

        public override void Step()
        {
            // Get coords
            Point p = CGrid.GetInDirection(Location, Direction);

            if (!Try(Direction,false,false))
            {   //Something's blocking us
                
                if (!CGrid.IsValid(p))
                {
                    Die();
                    return;
                }

                CElement target = CGrid.Get(p).GetTopmost();
                if (target != null && target.BlockBullets)
                {   
                    //Target is blocking bullets. fire Shot() and die.
                    target.Shot(Source, this);
                    Die();
                }
                else 
                    //Target is NOT blocking bullets, so we'll just go right ahead.
                    Move(Location, p);

            }
            else
                Move(Location, p);


        }

    }
}
