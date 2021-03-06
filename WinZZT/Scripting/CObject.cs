﻿/*

CObject.cs

Scriptable object! Woo.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinZZT
{
    class CObject : CTimedElement
    {

        public string Name;
        CScript Script;

        public bool Locked = false;

        public CObject(int x, int y, int chr, Color color, Color background, string name, string scriptID)
        {
            this.InitProps(x, y, chr, color, background, true, 995, 100);
            this.Name = name;

            string sScript = CMapManager.GetMapScript(scriptID);

            Script = new CScript(this, sScript);

            CWorldManager.CurrentMap.RegisterObject(this);

        }

        public override void Die()
        {
            base.Die();
            CWorldManager.CurrentMap.DeleteObject(this);
        }

        public void Put(EDirection d, CElementBlueprint bp)
        {

            Point p = CGrid.GetInDirection(Location, d);

            CElement.Create(p.X, p.Y, bp.Type, bp.Color);

        }


        public override void Step()
        {
            Script.Step();
        }

        public override void Shot(CElement responsible, CBullet bullet)
        {
            if (!Locked)
                Script.JumpToLabel("shot");
        }

        public override void Touch(CElement responsible)
        {
            if (!Locked)
                Script.JumpToLabel("touch");
        }

        public void Message(string m)
        {   //Sent by another object or script.
            if (!Locked)
                Script.JumpToLabel(m);
        }

    }
}
