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
            foreach (CElement c in elementList)
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

    }
}
