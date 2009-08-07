using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{

    class CAmmo : CElement
    {

        int Ammo;

        public CAmmo(int x, int y, int ammo)
        {
            this.InitProps(x, y, 132, Color.DarkCyan, Color.Transparent, false, 200);
            this.Ammo = ammo;
            this.CanBeSteppedOn = true;
        }

        public override void SteppedOn(CElement responsible)
        {
            if (responsible.IsPlayer)
            {
                CGame.PlayerAmmo += Ammo;
                this.Die();
            }
        }

    }

}
