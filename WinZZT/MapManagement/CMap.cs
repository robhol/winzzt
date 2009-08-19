using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;

namespace WinZZT
{
    class CMap
    {

        public string Name;
        XElement MapNode;

        private List<CElement> elementList;
        private Dictionary<string, CObject> objectList;

        public CGrid Grid;
        public bool FirstRun;

        public CMap(XElement mapNode)
        {

            Name = mapNode.Attribute("name").Value;

            MapNode = mapNode;

            elementList = new List<CElement>();
            objectList = new Dictionary<string, CObject>();

            Grid = new CGrid();
            Grid.Initialize();

            FirstRun = true;

        }

        public void ChangeTo(bool initial, bool spawnPlayer)
        {

            if (CWorldManager.CurrentMap != null)
                CWorldManager.CurrentMap.Unload();

            CWorldManager.CurrentMap = this;


            if (FirstRun) // Only load from map if it's the first time
                CMapManager.ProcessXMLMap(MapNode, spawnPlayer, initial);

            Load();

            FirstRun = false;
        }


        public void ChangeTo(Point p, bool initial)
        {
            ChangeTo(initial, false);
            CGame.SpawnPlayer(p.X, p.Y, false);
        }

        public void Load()
        {

            //If this is the first time the map loads, we don't need to do anything here.
            //This is pretty much "resume action" only.
            if (FirstRun)
                return;

            //Start all "paused" elements - copy to array, iterate and call where possible.
            CElement[] el = elementList.ToArray();

            foreach (CElement e in el)
            {

                if (e is CObject)
                {
                    ((CObject)e).Start();
                    continue;
                }

                if (e is CTimedElement)
                {
                    ((CTimedElement)e).Start();
                    continue;
                }

            }

        }

        public void Unload()
        {

            //Remove the player from the grid. Will be spawned when map loads later.
            CGame.Player.Die();

            //Loop through all elements. If object/timed, call Stop().
            //Again, to be safe, copy to array to avoid whining/exceptions
            CElement[] el = elementList.ToArray();

            foreach (CElement e in el)
            {

                if (e is CObject)
                {
                    ((CObject)e).Stop();
                    continue;
                }

                if (e is CTimedElement)
                {
                    ((CTimedElement)e).Stop();
                    continue;
                }

            }


        }

        /// <summary>
        /// Deletes all existing elements.
        /// </summary>
        public void DeleteAll()
        {
            //Copy to array to avoid exception
            CElement[] elements = elementList.ToArray();

            foreach (CElement c in elements)
                c.Die();

            elementList.Clear();
        }

        /// <summary>
        /// Adds a certain element to the list.
        /// </summary>
        /// <param name="e">The element to add</param>
        public void Register(CElement e)
        {
            elementList.Add(e);
        }

        public void RegisterObject(CObject o)
        {
            if (!objectList.ContainsKey(o.Name))
                objectList.Add(o.Name, o);
        }

        public void Delete(CElement e)
        {
            if (elementList.Contains(e))
                elementList.Remove(e);
        }

        public void DeleteObject(CObject e)
        {
            if (objectList.ContainsKey(e.Name))
                objectList.Remove(e.Name);
        }

        public CObject GetObjectFromName(string n)
        {
            if (objectList.ContainsKey(n))
                return objectList[n];

            return null;
        }

        public void SendMessageToObjects(CObject except, string msg)
        {

            foreach (KeyValuePair<string, CObject> p in objectList)
            {
                if (p.Value != except)
                    p.Value.Message(msg);
            }

        }

        public void SendMessageToObject(string name, string msg)
        {

            CObject o = GetObjectFromName(name);

            if (o != null)
                o.Message(msg);

        }

        public void Change(CElementBlueprint from, CElementBlueprint to)
        {

            // Copy to array. Avoids exceptions
            CElement[] eList = elementList.ToArray();

            foreach (CElement e in eList)
            {
                if (from.Match(e))
                    e.Become(to);
            }

        }







    }
}
