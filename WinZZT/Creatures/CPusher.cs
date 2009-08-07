using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    class CPusher : CTimedElement
    {

        EDirection pusherType;

        public CPusher(int x, int y, EDirection type, Color c)
        {
            int[] pusherChars = { 30, 16, 31, 17 };
            this.InitProps(x, y, pusherChars[(int)type], c, Color.Transparent, true, 61, 700);
            this.pusherType = type;
        }

        public override void Step()
        {
            Try(pusherType, true, true);
        }

    }
}
