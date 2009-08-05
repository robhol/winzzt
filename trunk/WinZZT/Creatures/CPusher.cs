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
            string[] pusherChars = { "▲", "►", "▼", "◄" };
            this.InitProps(x, y, pusherChars[(int)type], c, Color.Black, true, 61, 700);
            this.pusherType = type;
        }

        public override void Step()
        {
            Try(pusherType, true, true);
        }

    }
}
