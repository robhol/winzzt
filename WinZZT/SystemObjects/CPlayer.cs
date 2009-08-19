/*

CPlayer.cs

The player class...
  
*/

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
            this.CanBeSteppedOn = false;
            this.Pushable = true;

            this.InitProps(x, y, 2, Color.White, Color.DarkBlue, true, 10000);
        }

        private void TryShoot(EDirection d)
        {

            if (CGame.PlayerAmmo > 0)
            {


                if (Shoot(d))
                    CGame.PlayerAmmo--;
                else
                {
                    //check for breakables
                    if (this.HasElementInDirection("breakable", d))
                    {
                        //remove them
                        CWorldManager.CurrentMap.Grid.Get(CGrid.GetInDirection(Location, d)).Contents.RemoveAll(delegate(CElement e)
                        {
                            return e.Type == "breakable";
                        }
                        );

                        //subtract ammo
                        CGame.PlayerAmmo--;
                    }
                }


            }
            else
            {
                CDrawing.DisplayText("No ammo!", 1000);
            }
            
        }

        public void HandleInput(EDirection d, bool shoot)
        {

            if (!CGame.PlayerFrozen)
                //If frozen, we can't shoot or move...
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
        }

    }
}
