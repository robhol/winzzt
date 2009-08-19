/*
 
CDrawing.cs
 
Contains members and methods relating to drawing.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Timers;

using System.Collections;

namespace WinZZT
{

    static class CDrawing
    {
        public static Size CellSize = new Size(8, 14);

        public static Size CanvasSize = new Size(320, 350);

        //Font for text at the bottom of the screen.
        public static Font DrawFont = new Font("Trebuchet MS", 12);

        //Font used in the side bar/status bar
        public static Font BarFont = new Font("Courier New", 8, FontStyle.Bold);

        public static StringFormat stringFormat = new StringFormat();

        private static Dictionary<int, BitmapCharacter> DrawableCharacters = new Dictionary<int, BitmapCharacter>();

        [Obsolete]
        private static Dictionary<int, Bitmap> LoadBitmapCharacters()
        {
            Dictionary<int, Bitmap> characters = new Dictionary<int, Bitmap>();
            Bitmap currBitmap;

            for (int i = 1; i < 256; i++)
            {
                currBitmap = CCharManager.GetChar(i, Color.Wheat) as Bitmap;
                characters.Add(i, currBitmap);
            }

            return characters;
        }


        //Whether or not to draw the grid.
        public static bool DrawDGrid = false;

        public static Timer tmrDraw;

        public static void Initialize()
        {
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            tmrDraw = new Timer();
            tmrDraw.Interval = 30;
            tmrDraw.Elapsed += new ElapsedEventHandler(tmrDraw_Elapsed);
            tmrDraw.Start();
        }

        static DateTime textExpiration = new DateTime(1999, 1, 1);
        static string textValue = "";
        static bool textDisplayActive = false;

        static void tmrDraw_Elapsed(object sender, ElapsedEventArgs e)
        {
            //If text has expired, set text drawing flag off
            if (textExpiration < DateTime.Now)
                textDisplayActive = false;

            //Make sure to draw
            CDrawing.ProvokeDrawing(true);

        }

        /// <summary>
        /// Shows text at the bottom of the screen.
        /// </summary>
        /// <param name="s">Text to show</param>
        /// <param name="time">Duration in ms</param>
        public static void DisplayText(string s, double time)
        {
            textExpiration = DateTime.Now.AddMilliseconds(time);
            textValue = s;
            textDisplayActive = true;
        }

        /// <summary>
        /// Gets the canvas coords from grid coords
        /// </summary>
        /// <param name="p">Point in the grid</param>
        /// <returns></returns>
        public static Point GetCanvasCoords(Point p)
        {
            int x = p.X * CellSize.Width;
            int y = p.Y * CellSize.Height;
            return new Point(x, y);
        }

        public static Point GetTileFromCanvasCoords(Point p)
        {
            int x = (int)Math.Floor((double)p.X / CellSize.Width);
            int y = (int)Math.Floor((double)p.Y / CellSize.Height);
            return new Point(x, y);
        }

        public static void DrawSymbol(int c, Point p, Color foreground, Color background, Graphics g)
        {   //Draws the resemblance of an element. Mostly intended for sidebar use, etc.


            g.FillRectangle(new SolidBrush(background), p.X, p.Y, CellSize.Width, CellSize.Height);

            int toSearch = BitmapCharacter.CalculateSearch(foreground, c);

            BitmapCharacter loadedBitmap;
            if (!DrawableCharacters.ContainsKey(toSearch))
            {
                loadedBitmap = new BitmapCharacter(foreground, c);
                DrawableCharacters.Add(toSearch, loadedBitmap);
            }

            g.DrawImage(DrawableCharacters[toSearch].Bitmap, p);

        }

        public static void DrawObject(CElement o, Graphics g)
        {   // Draws a certain element

            if (o == null)
                return;

            Point p = GetCanvasCoords(o.Location);

            g.FillRectangle(new SolidBrush(o.BackColor), p.X, p.Y, CellSize.Width, CellSize.Height);

            int toSearch = BitmapCharacter.CalculateSearch(o.ForeColor, o.Char);

            BitmapCharacter loadedBitmap;
            if (!DrawableCharacters.ContainsKey(toSearch))
            {
                loadedBitmap = new BitmapCharacter(o.ForeColor, o.Char);
                DrawableCharacters.Add(toSearch, loadedBitmap);
            }

            g.DrawImage(DrawableCharacters[toSearch].Bitmap, p);
        }

        public static void DrawTile(int x, int y, Graphics g)
        {   //Draws a certain tile

            Point gp = new Point(x, y);
            Point p = GetCanvasCoords(gp);

            if (DrawDGrid)
            {   //Drawing the background grid

                Pen pn = new Pen(Color.FromArgb(16, 16, 16));
                g.DrawRectangle(pn, p.X, p.Y, CellSize.Width, CellSize.Height);
            }

            //If map is dark, prevent any drawing if outside of range
            if (CMapManager.MapIsDark)
            {
                if (CUtil.getDistance(gp,CGame.Player.Location) != 0 && (CUtil.getDistance(gp, CGame.Player.Location) >= 6.0 || !CGame.TorchActive))
                {
                    DrawSymbol(176, p, Color.Black, Color.FromArgb(64,64,64), g);
                    return;
                }
            }

            //Get tile
            CTile t = CWorldManager.CurrentMap.Grid.Get(gp);

            if (t.Contents.Count > 0)
            {   //If not empty, copy to array(avoids exceptions) and iterate, drawing everything.

                CElement[] ca = t.Contents.ToArray();

                foreach (CElement c in ca)
                    DrawObject(c, g);

            }


        }

        public static void DrawBar(Graphics g)
        {   //Draws the side bar

            StringFormat sfCenter = new StringFormat();
            sfCenter.LineAlignment = StringAlignment.Center;

            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 100)), CanvasSize.Width, 0, 175, CanvasSize.Height);
            g.DrawLine(new Pen(Color.FromArgb(0, 0, 60), 2), CanvasSize.Width, 0, CanvasSize.Width, CanvasSize.Height);

            //WinZZT Line
            g.DrawString("- WinZZT -", BarFont, Brushes.Yellow, CanvasSize.Width + 53, + 30, sfCenter);

            //Actual values
            g.DrawString("Health :  " + CGame.PlayerHealth.ToString().PadLeft(3, char.Parse(" ")), BarFont, Brushes.White, CanvasSize.Width + 30, 50);
            g.DrawString("Ammo   : " + CGame.PlayerAmmo.ToString().PadLeft(4, char.Parse(" ")), BarFont, Brushes.White, CanvasSize.Width + 30, 65);
            g.DrawString("Torches: " + CGame.PlayerTorches.ToString().PadLeft(4, char.Parse(" ")), BarFont, Brushes.White, CanvasSize.Width + 30, 80);
            g.DrawString("Gems   : " + CGame.PlayerGems.ToString().PadLeft(4, char.Parse(" ")), BarFont, Brushes.White, CanvasSize.Width + 30, 95);
            g.DrawString("Score  : " + CGame.PlayerScore.ToString().PadLeft(4, char.Parse(" ")), BarFont, Brushes.White, CanvasSize.Width + 30, 110);

            //Icons for health, ammo and torches.
            DrawSymbol(3, new Point(CanvasSize.Width + 20, 51), Color.FromArgb(255, 64, 72), Color.Transparent, g);
            DrawSymbol(132, new Point(CanvasSize.Width + 20, 66), Color.DarkCyan, Color.Transparent, g);
            DrawSymbol(157, new Point(CanvasSize.Width + 20, 81), Color.Brown, Color.Transparent, g);
            DrawSymbol(4, new Point(CanvasSize.Width + 20, 96), Color.Cyan, Color.Transparent, g);


            //Keys and numbers
            Point kdOffset = new Point(CanvasSize.Width + 12, CanvasSize.Height - 60);

            int d = 0;
            foreach (KeyValuePair<Color, int> k in CGame.KeyList)
            {
                Point p = CUtil.addPoints(kdOffset, new Point(25 * (int)Math.Floor((double)d / 4), 13 * (d % 4)));
                
                DrawSymbol(12, p, k.Key, Color.Transparent, g);
                
                if (k.Value > 1)
                    g.DrawString(k.Value.ToString(), BarFont, Brushes.White, p.X + 6, p.Y - 1);
                
                d++;
            }

            //Torch time if active
            if (CGame.TorchActive)
            {
                g.FillRectangle(new SolidBrush(Color.Black ), CanvasSize.Width + 125, 86, 30, 3);
                g.FillRectangle(new SolidBrush(Color.Orange), CanvasSize.Width + 125, 86, (int)Math.Round(28.0 * ((double)CGame.TorchTime / 20)), 3);
            }


        }

        public static void DrawGrid(Graphics g)
        {   //Draws the entire grid.

            for (int y = 0; y < CGrid.GridSize.Height; y++)
                for (int x = 0; x < CGrid.GridSize.Width; x++)
                {
                    DrawTile(x, y, g);
                }

            if (CGame.PlayerDead)
            {   //If the player is dead, draw a Game Over overlay...

                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;

                g.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 0, 0)), 0, 0, CanvasSize.Width, CanvasSize.Height);
                g.DrawString("GAME OVER", new Font("Courier New", 14, FontStyle.Bold), Brushes.White, CanvasSize.Width / 2, CanvasSize.Height / 2, f);
            }

            if (textDisplayActive)
            {   //If a text string is active, draw that too...

                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;

                g.DrawString(textValue, DrawFont, Brushes.White, new Rectangle(0, CanvasSize.Height - CellSize.Height - 7, CanvasSize.Width, CellSize.Height + 7), f);

            }

            DrawBar(g);

        }

        public static void ProvokeDrawing(bool force)
        {   //Forces drawing

            if (force && Program.MainForm != null)
                Program.MainForm.pb.Invalidate();
        }

    }

    /* Wrapper for the image */
    class BitmapCharacter
    {
        public BitmapCharacter(Color color, int character)
        {
            this.Bitmap = CCharManager.GetChar(character, color) as Bitmap;
            this.Color = color;
            this.Char = character;
        }

        public Bitmap Bitmap { get; set; }
        public Color Color { get; set; }
        public int Char { get; set; }

        public int SearchValue /* Unique search value for Dictionary */
        {
            get
            {
                return CalculateSearch(this.Color, this.Char);
            }
        }

        public static int CalculateSearch(Color color, int character)
        {
            return (color.R * 1000 + color.G * 100 + color.B * 10) + character;
        }
    }

}