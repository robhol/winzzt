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
    public partial class frmConsole : Form
    {
        public frmConsole()
        {
            InitializeComponent();
        }

        private void OutputConsole(string s)
        {
            txtConsole.Text += s + "\r\n";
        }

        private void HandleCommand(string raw)
        {

            string[] args = raw.Split(" ".ToCharArray());

            switch (args[0])
            {
                case "heal":
                    {

                        if (args.Length == 1)
                        {
                            CGame.PlayerHealth = 100;
                        }
                        else
                        {
                            CGame.PlayerHealth += int.Parse(args[1]);
                        }
                        

                        OutputConsole("Player healed.");
                        break;
                    }

                case "ammo":
                    {

                        if (args.Length == 1)
                        {
                            CGame.PlayerAmmo += 25;
                        }
                        else
                        {
                            CGame.PlayerAmmo += int.Parse(args[1]);
                        }


                        OutputConsole("Ammo given.");
                        break;
                    }

                case "clear":
                    {
                        CGrid.ClearGrid();
                        break;
                    }

                case "lm":
                    {
                        CMapManager.LoadMap(args[1]);
                        break;
                    }

                case "exit":
                    {
                        this.Close();
                        break;
                    }
                default:
                    {
                        OutputConsole("Unknown command.");
                        break;
                    }
            }


        }

        private void txtConsole_Enter(object sender, EventArgs e)
        {
            txtInput.Focus();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                HandleCommand(txtInput.Text);
                txtInput.Text = "";
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void frmConsole_Load(object sender, EventArgs e)
        {

        }
    }
}
