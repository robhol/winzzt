using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    static class CGrid
    {
        static private Dictionary<Point, CTile> DGrid = new Dictionary<Point, CTile>();
        static public Size GridSize = new Size(40, 25);

        static public void Initialize()
        {
            for (int y=0; y<GridSize.Height; y++)
                for (int x = 0; x < GridSize.Width; x++)
                {
                    DGrid.Add(new Point(x, y), new CTile());
                }
        }

        static public void ClearGrid()
        {
            DGrid.Clear();
            Initialize();
            CElementManager.DeleteAll();
            CGame.PlayerSpawned = false;
        }

        static public Point GetInDirection(Point o, EDirection d)
        {
            Point p = new Point(0, 0);
            switch (d)
            {
                case EDirection.North:
                    p = new Point(o.X, o.Y - 1);
                    break;
                case EDirection.South:
                    p = new Point(o.X, o.Y + 1);
                    break;
                case EDirection.West:
                    p = new Point(o.X - 1, o.Y);
                    break;
                case EDirection.East:
                    p = new Point(o.X + 1, o.Y);
                    break;
            }

            return p;

        }

        public static EDirection GetDirectionToPoint(Point from, Point to, bool alt)
        {

            int dX = from.X - to.X;
            int dY = from.Y - to.Y;

            if (Math.Abs(dX) > Math.Abs(dY) && !alt)
            {
                if (dX > 0)
                    return EDirection.West;
                return EDirection.East;
            }
            else
            {
                if (dY > 0)
                    return EDirection.North;
                return EDirection.South;
            }

        }

        public static EDirection GetOppositeDirection(EDirection d)
        {

            switch (d)
            {
                case EDirection.North:  return EDirection.South;
                case EDirection.East:   return EDirection.West;
                case EDirection.South:  return EDirection.North;
                case EDirection.West:   return EDirection.East;
            }

            return EDirection.East; // purely to avoid whining from the idiot compiler

        }

        static public bool IsValid(Point p)
        {
            return (p.X >= 0 && p.X < GridSize.Width) && (p.Y >= 0 && p.Y < GridSize.Height);
        }

        static public CTile Get(Point p)
        {
            return DGrid[p];
        }

        static public void Set(Point p, CTile t)
        {
            DGrid[p] = t;
        }


    }




}
