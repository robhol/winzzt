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
        private static Dictionary<string, CObject>  objectList;

        public static void Initialize()
        {
            elementList = new List<CElement>();
            objectList = new Dictionary<string, CObject>();
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

        public static void RegisterObject(CObject o)
        {
            if (!objectList.ContainsKey(o.Name))
                objectList.Add(o.Name, o);
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

        public static void DeleteObject (CObject e)
        {
            if (objectList.ContainsKey(e.Name))
                objectList.Remove(e.Name);
        }

        public static CObject GetObjectFromName(string n)
        {
            if (objectList.ContainsKey(n))
                return objectList[n];

            return null;
        }

        public static void SendMessageToObject(string name, string msg)
        {

            CObject o = GetObjectFromName(name);

            if (o != null)
                o.Message(msg);

        }

    }
}
