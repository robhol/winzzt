/*

CLine.cs

Wall, albeit with a different look.
Changes char depending on adjacent Line-type walls.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CLine : CElement
    {

        public CLine(int x, int y, Color c)
        {
            InitProps(x, y, 249, c, Color.Black, true, 0);

            //Check if there are "old" Lines adjacent and make them update
            for (int di = 0; di < 4; di++)
            {
                EDirection d = (EDirection)di;
                Point p = CGrid.GetInDirection(Location, d);

                CElement e = GetElementInDirection("line", d);

                if (e != null)
                {
                    ((CLine)e).CheckAdjacents();
                }

            }

            CheckAdjacents(); //Perform our own check too

        }

        public void CheckAdjacents()
        {
            Dictionary<EDirection, bool> adj = new Dictionary<EDirection, bool>();
            int num = 0;

            for (int di = 0; di < 4; di++)
            {
                EDirection d = (EDirection)di;
                
                bool h = HasElementInDirection("line", d);
                
                adj[d] = h;
                
                if (h)
                    num++;
            }

            CDrawing.DisplayText(Location.ToString() + " N=" + num.ToString(), 1000);

            if (num == 0) // Solitary...
                return;

            if (num == 1) // End point
            {
                if (adj[EDirection.North])
                    Char = 208;

                if (adj[EDirection.East])
                    Char = 198;

                if (adj[EDirection.South])
                    Char = 210;

                if (adj[EDirection.West])
                    Char = 181;

            }

            if (num == 2) // Connection
            {

                if (adj[EDirection.North] &&
                    adj[EDirection.South])
                    Char = 186;

                if (adj[EDirection.East] &&
                    adj[EDirection.West])
                    Char = 205;

                if (adj[EDirection.North] &&
                    adj[EDirection.East])
                    Char = 200;

                if (adj[EDirection.South] &&
                    adj[EDirection.East])
                    Char = 201;

                if (adj[EDirection.North] &&
                    adj[EDirection.West])
                    Char = 188;


                if (adj[EDirection.South] &&
                    adj[EDirection.West])
                    Char = 187;

            }

            if (num == 3) // Connection
            {

                // WNE
                if (adj[EDirection.West] &&
                    adj[EDirection.North] &&
                    adj[EDirection.East]
                    )
                    Char = 202;

                // NES
                if (adj[EDirection.North] &&
                    adj[EDirection.East] &&
                    adj[EDirection.South]
                    )
                    Char = 204;

                //ESW
                if (adj[EDirection.East] &&
                    adj[EDirection.South] &&
                    adj[EDirection.West]
                    )
                    Char = 203;

                //SWN
                if (adj[EDirection.South] &&
                    adj[EDirection.West] &&
                    adj[EDirection.North]
                    )
                    Char = 185;

            }

            if (num == 4)
                Char = 206;
            
        }

    }
}
