using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CLion : CTimedElement
    {

        public CLion(int x, int y)
        {
            this.InitProps(x, y, 234, Color.Orange, Color.Transparent, true, 61,700);
            
        }

        public override void Step()
        {
            Seek(CGame.Player);
        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            Die();
        }

    }
}
