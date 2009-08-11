/*

CKey.cs

Pickup for keys... 
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{

    class CKey : CElement
    {

        public CKey(int x, int y, Color c)
        {
            this.InitProps(x, y, 12, c, Color.Transparent, true, 200);
            this.CanBeSteppedOn = true;
        }

        public override bool SteppedOn(CElement responsible)
        {
            if (responsible.Type == "player") //Can't be picked up by non-players...
            {
                if (CGame.AddKey(this.ForeColor))
                {
                    this.Die();
                    return true;
                }
            }

            return false;
        }

    }

}
