/*

CDoor.cs

Doors. Get the right key and open them.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CDoor : CElement
    {
        public CDoor(int x, int y, Color c)
        {
            InitProps(x, y, 9, c, Color.White, true, 998);
            this.CanBeSteppedOn = true;
        }

        public override bool SteppedOn(CElement responsible)
        {
            if (responsible.Type != "player")
                return false;

            if (CGame.UseKey(this.ForeColor))
            {
                this.Die();
                return true;
            }

            return false;

        }


    }
}
