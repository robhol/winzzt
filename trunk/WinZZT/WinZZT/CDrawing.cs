using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Timers;

namespace WinZZT
{
    static class CDrawing
    {

        public static Size CellSize = new Size(12, 16);

        public static Size CanvasSize = new Size(612, 433);

        public static Size CellSpacing = new Size(0, 0);

        public static Font DrawFont = new Font("Lucida Console", 12);

        public static StringFormat stringFormat = new StringFormat();

        public static bool DrawDGrid = true;

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

        static DateTime textExpiration = new DateTime(1999,1,1);
        static string textValue = "";
        static bool textDisplayActive = false;

        static void tmrDraw_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (textExpiration < DateTime.Now)
                textDisplayActive = false;

            CDrawing.ProvokeDrawing(true);

        }

        public static void DisplayText(string s, double time)
        {
            textExpiration = DateTime.Now.AddMilliseconds(time);
            textValue = s;
            textDisplayActive = true;
        }

        public static Point GetCanvasCoords(Point p)
        {
            int x = p.X * CellSize.Width + CellSpacing.Width;
            int y = p.Y * CellSize.Height + CellSpacing.Height;
            return new Point(x, y);
        }

        public static void DrawObject(CElement o, Graphics g)
        {

            if (o == null)
                return;

            Point p = GetCanvasCoords(o.Location);

            g.FillRectangle(new SolidBrush(o.BackColor), p.X, p.Y, CellSize.Width, CellSize.Height);

            g.DrawString(o.Char, DrawFont, new SolidBrush(o.ForeColor), p.X + (CellSize.Width / 2), p.Y + (CellSize.Height / 2), stringFormat);

        }

        public static void DrawTile(int x, int y, Graphics g)
        {
            Point gp = new Point(x,y);
            Point p = GetCanvasCoords(gp);

            CTile t = CGrid.Get(gp);

            if (t.Contents.Count > 0)
            {
                DrawObject(t.GetTopmost(), g);
            }

            if (DrawDGrid)
            {
                Pen pn = new Pen(Color.FromArgb(16, 16, 16));
                g.DrawRectangle(pn, p.X, p.Y, CellSize.Width, CellSize.Height);
            }
        }

        public static void DrawGrid(Graphics g)
        {
            for (int y = 0; y < CGrid.GridSize.Height; y++)
                for (int x = 0; x < CGrid.GridSize.Width; x++)
                {
                    DrawTile(x, y, g);
                }

            if (CGame.PlayerDead)
            {
                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;

                g.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 0, 0)), 0, 0, CanvasSize.Width, CanvasSize.Height);
                g.DrawString("GAME OVER", new Font("Courier New", 14), Brushes.White, CanvasSize.Width / 2, CanvasSize.Height / 2,f);
            }

            if (textDisplayActive)
            {

                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;

                g.DrawString(textValue, DrawFont, Brushes.White, new RectangleF(0, CanvasSize.Height - CellSize.Height, CanvasSize.Width, CellSize.Height),f);

            }

        }


        public static void ProvokeDrawing( bool force )
        {
            if (force && Program.MainForm != null)
            Program.MainForm.pb.Invalidate();
        }

    }
}
