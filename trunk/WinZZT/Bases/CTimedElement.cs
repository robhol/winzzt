using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace WinZZT
{
    class CTimedElement : CElement
    {
        private Timer tmr;
        private double _cycle = 500;

        public void InitTimer(double cycle)
        {
            _cycle = cycle;
            tmr = new Timer(_cycle);
            tmr.Elapsed += new ElapsedEventHandler(tmrStep);
            tmr.Start();
        }

        public void InitProps(int x, int y, string c, System.Drawing.Color foreground, System.Drawing.Color background, bool block, int ordering, double cycle)
        {
            base.InitProps(x, y, c, foreground, background, block, ordering);
            InitTimer(cycle);
        }

        public int Cycle
        {
            get { return (int)_cycle; }
            set { _cycle = (double)value; }
        }

        public virtual void Step()
        {

        }

        public override void Die()
        {
            base.Die();
            tmr.Enabled = false;
        }

        private void tmrStep(object sender, ElapsedEventArgs e)
        {
            Step();
        }


    }
}
