/*

CPassage.cs

"Portal" from a map to another.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{

    class CPassage : CElement
    {

        public string Destination;
        public Point DestinationPoint;

        public CPassage(int x, int y, Color c, string destination, Point destPoint)
        {
            this.InitProps(x, y, 240, Color.LightGray, c, true, 256);
            this.CanBeSteppedOn = true;

            Destination = destination;
            DestinationPoint = destPoint;
        }

        public override bool SteppedOn(CElement responsible)
        {
            if (responsible.Type == "player") //Can't be stepped on by non-players...
            {
                CWorldManager.ChangeMap(Destination, DestinationPoint);
            }

            return false;
        }

    }

}
