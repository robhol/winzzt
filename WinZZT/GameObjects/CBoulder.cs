﻿/*

CBoulder.cs

Your basic boulder. Easy to push around.
  
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CBoulder : CElement
    {

        public CBoulder(int x, int y, Color c)
        {

            this.ForeColor = c;
            this.BackColor = Color.Transparent;
            this.Char = 254;
            this.Block = true;
            this.Pushable = true;
            this.Ordering = 500;

            this.Initialize(x, y);

        }

        public override void  Touch(CElement responsible)
        {
            
        }

    }
}
