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
            this.BackColor = Color.FromArgb(0,0,0,0);
            this.ForeColor = Color.White;
            this.Char = "∙";
            this.Block = true;

            this.Source = source;

            this.Ordering = 999;

            Direction = direction;
            this.Cycle = cCycle;
            this.Location = new Point(x, y);
            base.InitPosition(x, y);
            base.InitTimer(cCycle);
        }

        public override void Step()
        {

            Point p = CGrid.GetInDirection(Location, Direction);

            if (!Try(Direction,false,false))
            {
                

                if (!CGrid.IsValid(p))
                {
                    Die();
                    return;
                }


                    CElement target = CGrid.Get(p).GetTopmost();
                    if (target != null && target.BlockBullets)
                    {
                        target.Shot(Source, this);
                        Die();
                    }
                    else
                        Move(Location, p);

            }
            else
                Move(Location, p);


        }

    }
}
