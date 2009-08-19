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
                    //If we're at the edge of a map, check for links
                    if ((
                        (d == EDirection.West && Location.X == 0) ||
                        (d == EDirection.North && Location.Y == 0) ||
                        (d == EDirection.East && Location.X == CGrid.GridSize.Width - 1) ||
                        (d == EDirection.South && Location.Y == CGrid.GridSize.Height - 1)
                        ) && CWorldManager.CurrentMap.HasMapLinkInDirection(d)
                       )
                    {

                        Point p = Location;
                        string mapName = CWorldManager.CurrentMap.mapLinks[d];

                        //Find coords
                        switch (d)
                        {
                            case EDirection.North:
                                p.Y = CGrid.GridSize.Height - 1; break;

                            case EDirection.East:
                                p.X = 0; break;

                            case EDirection.South:
                                p.Y = 0; break;

                            case EDirection.West:
                                p.X = CGrid.GridSize.Width - 1; break;

                        }

                        //Check for blockages...
                        if (!CWorldManager.getMap(mapName).Grid.Get(p).IsBlocked())
                        {
                            //Go!
                            CWorldManager.ChangeMap(mapName, p);
                        }




                    }
                    else
                    {
                        //Normal walk
                        Try(d, true, true);
                    }
                }   
        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            CGame.DamagePlayer(10);
        }

    }
}
