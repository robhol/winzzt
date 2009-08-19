/*

CConveyor.cs

Moves stuff around
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CConveyor : CTimedElement
    {

        //Store the list of chars we're using, once and for all
        static int[] charList = new int[] { 179, 47, 45, 92 };

        //This seemed like a decent idea of looping through surrounding tiles.
        static Point[] surroundings = new Point[]
        {
            new Point(0,-1), //N
            new Point(1,-1), //NE
            new Point(1,0),  //E
            new Point(1,1),  //SE
            new Point(0,1),  //S
            new Point(-1,1), //SW
            new Point(-1,0), //W
            new Point(-1,-1) //NW
        };

        //A list of elements this conveyor has moved the current step.
        List<CElement> processed = new List<CElement>();

        EConveyorType convType;
        int charIndex;

        public CConveyor(int x, int y, EConveyorType type, Color c)
        {
            this.InitProps(x, y, 179, c, Color.Transparent, true, 61, 200);
            this.convType = type;
            this.charIndex = 0;
        }

        public override void Step()
        {

            //Clear processed list
            processed.Clear();

            //Scan surroundings, make any adjacent element move according to rotation.
            foreach (Point s in surroundings)
            {
                Point p = CUtil.addPoints(Location, s);
                CTile t = CWorldManager.CurrentMap.Grid.Get(p);

                if (t.Contents.Count == 0) //Skip this cell if empty
                    continue;

                //Get element
                CElement e = t.GetTopmost();

                if (e.Block == true && !e.Pushable) //Assume "permanent position," skip.
                    continue;

                if (processed.Contains(e)) //Skip if already processed
                    continue;

                int a = (int)Math.Round(CUtil.getRotationToPoint(Location, p));

                /*EDirection md = EDirection.North; //again.. default value to prevent compiler whining
                bool set = true;
                switch (a)
                {
                    case 0: md = EDirection.East; break;
                    case 45: md = EDirection.South; break;
                    case 90: md = EDirection.South; break;
                    case 135: md = EDirection.West; break;
                    case 180: md = EDirection.West; break;
                    case 225: md = EDirection.North; break;
                    case 270: md = EDirection.North; break;
                    case 315: md = EDirection.East; break;
                    default: set = false; break;
                }*/

                EDirection md = CUtil.getDirectionFromRotation(a + 90);

                if (convType == EConveyorType.Clockwise)
                    md = CUtil.getDirectionFromRotation(a+90);
                else
                    md = CUtil.getDirectionFromRotation(a-135);

                processed.Add(e);

                e.Try(md, true, false);

            }


            //Cycle chars
            if (convType == EConveyorType.Clockwise)
            {
                charIndex++;
                if (charIndex >= charList.Length)
                    charIndex = 0;
            }
            else
            {
                charIndex--;
                if (charIndex < 0)
                    charIndex = charList.Length-1;
            }

            this.Char = charList[charIndex];

        }

    }
}
