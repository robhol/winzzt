﻿using System;
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

            CMapManager.LoadMapFile("test");

        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {

            bool move = true;

            if (CGame.Player == null || !CGame.PlayerSpawned)  //Map probably not loaded
                move = false;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        if (move)
                            CGame.Player.HandleInput(EDirection.West, e.Shift);

                        break;
                    }
                case Keys.Right:
                    {
                        if (move)
                            CGame.Player.HandleInput(EDirection.East, e.Shift);
                        
                        break;
                    }
                case Keys.Up:
                    {
                        if (move)
                            CGame.Player.HandleInput(EDirection.North, e.Shift);

                        break;
                    }
                case Keys.Down:
                    {
                        if (move)
                            CGame.Player.HandleInput(EDirection.South, e.Shift);

                        break;
                    }
                case Keys.C:
                    {
                        if (e.Control && e.Alt)
                        {
                            frmConsole fc = new frmConsole();

                            fc.Location = new Point(this.Location.X + this.Width, this.Location.Y + this.Height - fc.Height);

                            fc.ShowDialog(this);
                        }
                        break;
                    }

            }
        }

    }
}
