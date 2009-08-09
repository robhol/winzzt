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
