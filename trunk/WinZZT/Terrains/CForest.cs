/*

CForest.cs

Forest. Step on once, it's gone forever.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CForest : CElement
    {
        public CForest(int x, int y)
        {
            InitProps(x, y, 178, Color.ForestGreen, Color.Black, true, 5); //Forest green. Ha.
            this.CanBeSteppedOn = true;
        }

        public override bool SteppedOn(CElement responsible)
        {
            if (responsible.Type == "player")
            {
                this.Die();
                return true;
            }

            return false;
        }

    }
}
