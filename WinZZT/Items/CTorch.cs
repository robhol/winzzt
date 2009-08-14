/*

CTorch.cs

Basic torch pickup.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{

    class CTorch : CElement
    {

        public CTorch(int x, int y)
        {
            this.InitProps(x, y, 157, Color.Brown, Color.Transparent, true, 205);
            this.CanBeSteppedOn = true;
        }

        public override bool SteppedOn(CElement responsible)
        {
            if (responsible.Type == "player") //Can't be picked up by non-players...
            {
                CGame.PlayerTorches++;
                this.Die();
                return true;
            }

            return false;
        }

    }

}
