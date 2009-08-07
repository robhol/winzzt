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

        public static Size CellSize = new Size(8, 14);

        public static Size CanvasSize = new Size(320, 350);

        public static Font DrawFont = new Font("Trebuchet MS", 12);

        public static Font BarFont = new Font("Courier New", 8); 

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
            int x = p.X * CellSize.Width;
            int y = p.Y * CellSize.Height;
            return new Point(x, y);
        }

        public static void DrawObject(CElement o, Graphics g)
        {

            if (o == null)
                return;

            Point p = GetCanvasCoords(o.Location);

            g.FillRectangle(new SolidBrush(o.BackColor), p.X, p.Y, CellSize.Width, CellSize.Height);

            g.DrawImage(CCharManager.GetChar(o.Char, o.ForeColor), p);

        }

        public static void DrawTile(int x, int y, Graphics g)
        {
            Point gp = new Point(x,y);
            Point p = GetCanvasCoords(gp);

            if (DrawDGrid)
            {
                Pen pn = new Pen(Color.FromArgb(16, 16, 16));
                g.DrawRectangle(pn, p.X, p.Y, CellSize.Width, CellSize.Height);
            }

            CTile t = CGrid.Get(gp);

            if (t.Contents.Count > 0)
            {
                DrawObject(t.GetTopmost(), g);
            }

            
        }

        public static void DrawBar(Graphics g)
        {

            StringFormat sfCenter = new StringFormat();
            sfCenter.LineAlignment = StringAlignment.Center;

            g.FillRectangle(new SolidBrush(Color.FromArgb(0,0,100)), CanvasSize.Width, 0, 100, CanvasSize.Height);

            g.DrawString("- WinZZT -", BarFont, Brushes.Yellow,CanvasSize.Width + 18, 30, sfCenter);

            string status =
                "Health :  " + CGame.PlayerHealth.ToString().PadLeft(3,char.Parse(" ")) + "\r\n" +
                "Ammo   : "  + CGame.PlayerAmmo.ToString().PadLeft(4, char.Parse(" "))
                ;

            g.DrawString(status, BarFont, Brushes.White, CanvasSize.Width + 5, 50);

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
                g.DrawString("GAME OVER", new Font("Courier New", 14, FontStyle.Bold), Brushes.White, CanvasSize.Width / 2, CanvasSize.Height / 2,f);
            }

            if (textDisplayActive)
            {

                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;

                g.DrawString(textValue, DrawFont, Brushes.White, new Rectangle(0, CanvasSize.Height - CellSize.Height - 7, CanvasSize.Width, CellSize.Height + 7),f);

            }

            DrawBar(g);

        }


        public static void ProvokeDrawing( bool force )
        {
            if (force && Program.MainForm != null)
            Program.MainForm.pb.Invalidate();
        }

    }
}
