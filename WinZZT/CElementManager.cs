using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinZZT
{
    static class CElementManager
    {

        private static List<CElement> elementList;

        public static void Initialize()
        {
            elementList = new List<CElement>();
        }

        public static void DeleteAll()
        {
            foreach (CElement c in elementList)
                c.Die();

            elementList.Clear();
        }

        public static void Register(CElement e)
        {
            elementList.Add(e);
        }

    }
}
