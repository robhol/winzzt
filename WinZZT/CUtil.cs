﻿/*

CUtil.cs

Functions that would be useful, but not fit too well at other places.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    static class CUtil
    {

        /// <summary>
        /// Parses a 6-/8-digit color string in the RRGGBB or RRGGBBAA format.
        /// </summary>
        /// <param name="s">The color string.</param>
        /// <returns></returns>
        public static Color getColorFromString(string s)
        {

            if (s == String.Empty)
                return Color.Transparent;

            int r = int.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            int a = 255;

            if (s.Length == 8) //if getting alpha
                a = int.Parse(s.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);

        }

        public static bool getColorFromString(string s, out Color c)
        {

            if (s == "")
            {
                c = Color.Transparent;
                return true;
            }

            int r, g, b, a;

            try
            {
                r = int.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                g = int.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                b = int.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                a = 255;

                if (s.Length == 8) //if getting alpha
                    a = int.Parse(s.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                c = Color.Transparent;
                return false;
            }

            c = Color.FromArgb(a, r, g, b);
            return true;
        }


        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point addPoints(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static double getDistance(Point a, Point b)
        {

            int dX = b.X - a.X;
            int dY = b.Y - a.Y;

            return Math.Sqrt( Math.Pow(dX, 2) + Math.Pow(dY, 2));

        }

        public static double getRotationToPoint(Point a, Point b)
        {
            double dX = (double)(b.X - a.X);
            double dY = (double)(b.Y - a.Y);

            double ang = 90 + (Math.Atan2(dY, dX) * (180.0 / Math.PI));

            if (ang < 0)
                ang += 360.0;

            return ang;
        }

        public static double getRotationFromDirection(EDirection d)
        {

            switch (d)
            {
                case EDirection.North: return 0;
                case EDirection.East: return 90;
                case EDirection.South: return 180;
                case EDirection.West: return 270;
            }

            return -1;

        }

        public static EDirection getDirectionFromRotation(double angle)
        {
            int a = (int)angle;

            if (a < 0)
                a += 360;
            else if (a >= 360)
                a -= 360;

            if (a >= 45 && a < 135)
                return EDirection.East;

            if (a >= 135 && a < 225)
                return EDirection.South;

            if (a >= 225 && a < 315)
                return EDirection.West;

            return EDirection.North;

        }

        public static EDirection getDirectionFromString(string s)
        {
            switch (s.ToUpper())
            {
                case "N": return EDirection.North;
                case "E": return EDirection.East;
                case "S": return EDirection.South;
                case "W": return EDirection.West;
            }

            return EDirection.North;

        }

        public static string getDirectionString(EDirection e)
        {
            switch (e)
            {
                case EDirection.North: return "N";
                case EDirection.East: return "E";
                case EDirection.South: return "S";
                case EDirection.West: return "W";
            }

            return "X";

        }

        public static string stripVariables(string s)
        {

            int idx = s.IndexOf("@");

            while (idx != -1)
            {
                int spi = s.IndexOf(" ", idx);

                if (spi != -1)
                {
                    s = s.Substring(0, idx) + "(undefined)" + s.Substring(spi);
                }
                else
                    s = s.Substring(0, idx) + "(undefined)";

                idx = s.IndexOf("@");

            }

            return s;
        }

        public static string getAligned(Point a, Point b)
        {
            if (a.X - b.X == 0 || a.Y - b.Y == 0)
                return "1";
            else
                return "0";
        }


    }
}
