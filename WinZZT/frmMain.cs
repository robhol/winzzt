using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinZZT
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void pb_Paint(object sender, PaintEventArgs e)
        {
            CDrawing.DrawGrid(e.Graphics);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
           
            CGame.SpawnPlayer(20, 15);

            new CTarget(30, 15);

            new CTiger(23, 7);

            new CBoulder(10, 10, Color.White);
            new CSlider(13, 10, ESliderType.Horizontal, Color.LightBlue);
            new CSlider(10, 13, ESliderType.Vertical, Color.LightGreen);

            for (int y = 5; y < 21; y++)
                new CInvisibleWall(22, y);

        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        CGame.Player.HandleInput(EDirection.West, e.Shift);   
                        break;
                    }
                case Keys.Right:
                    {
                        CGame.Player.HandleInput(EDirection.East, e.Shift);   
                        break;
                    }
                case Keys.Up:
                    {
                        CGame.Player.HandleInput(EDirection.North, e.Shift);   
                        break;
                    }
                case Keys.Down:
                    {
                        CGame.Player.HandleInput(EDirection.South, e.Shift);   
                        break;
                    }


            }
        }

        private void pb_Click(object sender, EventArgs e)
        {

        }

        
    }
}
