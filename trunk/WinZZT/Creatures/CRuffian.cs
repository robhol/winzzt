﻿/*

CRuffian.cs

Known for their improbable and sporadic movements.
  
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CRuffian : CTimedElement
    {

        public CRuffian(int x, int y)
        {
            this.InitProps(x, y, 5, Color.DarkMagenta, Color.Transparent, true, 61,100);
        }

        EDirection moveDirection;
        int moves = 0;
        bool moving = false;

        DateTime restUntil = DateTime.Now.AddMilliseconds(1000);

        public override void Step()
        {

            if (moves > 0)
            {
                moveDirection = CGrid.GetDirectionToPoint(Location, CGame.Player.Location, CGame.Random.Next(3) == 0);
                
                Try(moveDirection, true, false);
                moves--;
                moving = true;
            }
            else
            {
                if (moving) // if moving at the last tick
                {
                    restUntil = DateTime.Now.AddMilliseconds(CGame.Random.Next(1000,2000));
                    moving = false;
                }
            }

            if (restUntil < DateTime.Now && !moving)
            {
                moves = CGame.Random.Next(3, 7);
            }

        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            Die();
        }

    }
}
