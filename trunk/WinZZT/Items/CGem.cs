/*

CGem.cs

Basic gem pickup.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{

    class CGem : CElement
    {

        int Gems;

        public CGem(int x, int y, int amount)
        {
            this.InitProps(x, y, 4, Color.Cyan, Color.Transparent, true, 200);
            this.Gems = amount;
            this.CanBeSteppedOn = true;
        }

        public override bool SteppedOn(CElement responsible)
        {
            if (responsible.Type == "player") //Can't be picked up by non-players...
            {
                CGame.PlayerGems += Gems;
                this.Die();
                return true;
            }

            return false;
        }

    }

}
