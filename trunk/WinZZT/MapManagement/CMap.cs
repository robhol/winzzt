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

        public CMap(XElement mapNode)
        {

            Name = mapNode.Attribute("name").Value;

            MapNode = mapNode;
        }

        public void ChangeTo(bool initial)
        {
            CMapManager.ProcessXMLMap(MapNode, true, initial);
        }


        public void ChangeTo(Point p, bool initial)
        {
            CMapManager.ProcessXMLMap(MapNode, false, initial);
            CGame.SpawnPlayer(p.X, p.Y, false);
        }

    }
}
