/*

CElementManager.cs

Has a list over all existing elements.
This is mostly to make mass iteration easier.
  
*/

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

        /// <summary>
        /// Deletes all existing elements.
        /// </summary>
        public static void DeleteAll()
        {
            //Copy to array to avoid exception
            CElement[] elements = elementList.ToArray();

            foreach (CElement c in elements)
                c.Die();

            elementList.Clear();
        }

        /// <summary>
        /// Adds a certain element to the manager.
        /// </summary>
        /// <param name="e">The element to add</param>
        public static void Register(CElement e)
        {
            elementList.Add(e);
        }

        public static string Dump()
        {
            string o = "";

            foreach (CElement e in elementList)
                o += e.Location.X + "x" + e.Location.Y + " (" + e.Type + ")";
            
            return o;
        }

        public static void Delete(CElement e)
        {
            if (elementList.Contains(e))
                elementList.Remove(e);
        }

    }
}
