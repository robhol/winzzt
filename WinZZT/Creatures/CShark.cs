using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CShark : CTimedElement
    {

        public CShark(int x, int y)
        {
            this.InitProps(x, y, 94, Color.White, Color.Transparent, true, 61,700);
        }

        public override void Step()
        {
            //Check for player
            if (this.IsTouching(CGame.Player))
            {
                CGame.DamagePlayer(10);
                Die();
                return;
            }

            //Find directions with water in them
            List<int> possible = new List<int>();

            for (int di = 0; di < 4; di++)
            {
                if (this.HasElementInDirection("water", (EDirection)di))
                    possible.Add(di);
            }

            if (possible.Count == 0)
                return; //No options

            EDirection md = (EDirection)possible[CGame.Random.Next(possible.Count)];
            this.Move(Location, CGrid.GetInDirection(Location, md));

        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            Die();
        }

    }
}
