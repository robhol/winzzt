/*

CTimedElement.cs

Base class for all elements that use the Step() function or need timing.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace WinZZT
{
    class CTimedElement : CElement
    {

        #region "Members"

        private Timer tmr;
        private double _cycle = 500;

        public int Cycle
        {
            get { return (int)_cycle; }
            set 
            { 
                _cycle = (double)value;
                tmr.Interval = _cycle;
            }
        }

        #endregion

        #region "Creation and destruction"

        /// <summary>
        /// Initializes the timer for the given element.
        /// </summary>
        /// <param name="cycle">Millisecond interval between ticks/steps</param>
        public void InitTimer(double cycle)
        {
            _cycle = cycle;
            tmr = new Timer(_cycle);
            tmr.Elapsed += new ElapsedEventHandler(tmrStep);
            tmr.Start();
        }

        public void InitProps(int x, int y, int c, System.Drawing.Color foreground, System.Drawing.Color background, bool block, int ordering, double cycle)
        {
            base.InitProps(x, y, c, foreground, background, block, ordering);
            InitTimer(cycle);
        }

        public override void Die()
        {
            base.Die();

            //Stop and free up the timer
            tmr.Stop();
            tmr.Dispose();
        }

        #endregion

        #region "Event handling"

        private void tmrStep(object sender, ElapsedEventArgs e)
        {
            //This is only here so we won't have the handler arguments
            //over our heads in the main function.
            Step();
        }

        /// <summary>
        /// Triggered on timer ticks
        /// </summary>
        public virtual void Step()
        {

        }

        #endregion

    }
}
