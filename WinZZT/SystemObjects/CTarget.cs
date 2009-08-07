using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CTarget : CTimedElement
    {

        public CTarget(int x, int y)
        {

            this.ForeColor = Color.Gray;
            this.BackColor = Color.Transparent;
            this.Char = 111;
            this.Block = true;

            this.InitPosition(x, y);
            this.InitTimer(500);

        }

        int pendingShots = 0;
        DateTime dshoot = DateTime.MinValue;
        EDirection shootNext = EDirection.North;

        public override void Step()
        {
            if (pendingShots > 0 && DateTime.Now >= dshoot)
            {
                Shoot(shootNext);
                pendingShots--;
            }
        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            Random r = new Random();
            
            ForeColor = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));

            shootNext = CGrid.GetOppositeDirection(bullet.Direction);
            dshoot = DateTime.Now.AddMilliseconds(1000);
            pendingShots++;

            CDrawing.DisplayText("Target - Shot", 1000);
        }

        public override void Touch()
        {
            Random r = new Random();
            this.ForeColor = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CDrawing.DisplayText("Target - Touched", 1000);
        }

    }
}
