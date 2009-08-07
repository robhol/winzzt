using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CPlayer : CElement
    {

        public CPlayer(int x, int y)
        {
            this.Location = new Point(x, y);
            this.Char = 2;
            this.BackColor = Color.DarkBlue;
            this.ForeColor = Color.White;
            this.CanBeSteppedOn = false;
            this.Block = true;
            this.Pushable = true;
            this.Ordering = 10000;
            this.InitPosition(x, y);
            this.IsPlayer = true;
        }

        private void TryShoot(EDirection d)
        {

            if (CGame.PlayerAmmo > 0)
            {
                if (Shoot(d))
                    CGame.PlayerAmmo--;
            }
            else
            {
                CDrawing.DisplayText("No ammo!", 1000);
            }
            
        }

        public void HandleInput(EDirection d, bool shoot)
        {
            if (!CGame.PlayerFrozen)
                if (shoot)
                {
                    TryShoot(d);
                }
                else
                {
                    Try(d,true,true);
                }   
        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            CGame.DamagePlayer(10);
            CDrawing.DisplayText("Player shot: " + CGame.PlayerHealth.ToString() + "/100",500);
        }

    }
}
