/*

CBear.cs

Rawr.
Follows the player if aligned.
Damages and dies on collision.
Also will destroy breakable walls (and self) upon contact.
  
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CBear : CTimedElement
    {

        public CBear(int x, int y)
        {
            this.InitProps(x, y, 148, Color.Brown, Color.Transparent, true, 61,700);
        }

        public override void Step()
        {
            if (this.IsTouching(CGame.Player))
            {
                CGame.DamagePlayer(10);
                this.Die();
            }
            else
            {
                if (Location.X - CGame.Player.Location.X == 0 || Location.Y - CGame.Player.Location.Y == 0)
                {   //Aligned with player. Move. Discard Seek() in favor of own solution.

                    EDirection d = CGrid.GetDirectionToPoint(Location, CGame.Player.Location, false);

                    if (!this.HasElementInDirection("breakable", d))
                        Try(d, true, false);
                    else //If there's a breakable here...
                    {
                        //Get the tile
                        CTile t = CGrid.Get(CGrid.GetInDirection(Location, d));

                        //Destroy all breakables in the tile.
                        t.Contents.RemoveAll(
                            delegate(CElement e)
                            {
                                return e.Type == "breakable";
                            }
                        );

                        Die();

                    }
                }
            }
        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            Die();
        }

    }
}
