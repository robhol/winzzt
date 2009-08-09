/*

CTile.cs

Each "compartment" in the grid. Contains some basic checking functions.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinZZT
{
    class CTile
    {
        public List<CElement> Contents;

        public CTile()
        {
            Contents = new List<CElement>();
        }

        /// <summary>
        /// Gets the topmost element in a tile
        /// </summary>
        public CElement GetTopmost()
        {

            int t = -1;
            CElement e = null;

            CElement[] cnt = new CElement[Contents.Count];
            Contents.CopyTo(cnt);

            foreach (CElement ie in cnt)
            {
                if (ie != null && ie.Ordering > t)
                {
                    t = ie.Ordering;
                    e = ie;
                }
            }

            return e;

        }

        /// <summary>
        /// Checks if the tile is blocked.
        /// </summary>
        /// <returns></returns>
        public bool IsBlocked()
        {
            bool tmp = false;

            CElement[] cnt = new CElement[Contents.Count];
            Contents.CopyTo(cnt);

                foreach (CElement ie in cnt)
                    if (ie != null)
                        tmp = tmp || ie.Block;

            return tmp;
        }

        /// <summary>
        /// Checks if the tile contains an element of the given type
        /// </summary>
        /// <param name="t">Type to look for</param>
        /// <returns></returns>
        public bool ContainsType(string t)
        {
            CElement[] cnt = new CElement[Contents.Count];
            Contents.CopyTo(cnt);
            
            foreach (CElement ie in cnt)
                if (ie != null && ie.Type == t)
                    return true;
            
            return false;
        }

    }


}
