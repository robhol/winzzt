/*

CGrid.cs

Controls the entire game grid.
Provides methods for working out directions, coordinates and checking.
  
*/

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

        /// <summary>
        /// Gets the point in a given direction
        /// </summary>
        /// <param name="o">Point to start from.</param>
        /// <param name="d">Direction.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the direction from point A to B.
        /// </summary>
        /// <param name="from">Point A</param>
        /// <param name="to">Point B</param>
        /// <param name="alt">Force another direction</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the opposite direction from the given one.
        /// </summary>
        /// <param name="d">Direction.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if a point is a valid grid position.
        /// </summary>
        /// <param name="p">Point to check.</param>
        /// <returns></returns>
        static public bool IsValid(Point p)
        {
            return (p.X >= 0 && p.X < GridSize.Width) && (p.Y >= 0 && p.Y < GridSize.Height);
        }

        /// <summary>
        /// Gets the tile for a certain point.
        /// </summary>
        /// <param name="p">Point.</param>
        /// <returns></returns>
        static public CTile Get(Point p)
        {
            return DGrid[p];
        }

    }

}
