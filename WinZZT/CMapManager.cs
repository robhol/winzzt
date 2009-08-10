/*

CMapManager.cs

Handles map loading.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace WinZZT
{
    static class CMapManager
    {

        public static void HandleMapNode(XElement e)
        {

            int x = int.Parse(e.Attribute("x").Value);
            int y = int.Parse(e.Attribute("y").Value);

            switch (e.Name.ToString())
            {
                #region "Terrains"
                case "wall":
                    {
                        CElementManager.Register(new CWall(x, y, CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }
                case "breakable":
                    {
                        CElementManager.Register(new CBreakable(x, y, CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }
                case "normal":
                    {
                        CElementManager.Register(new CNormal(x, y, CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }
                case "fake":
                    {
                        CElementManager.Register(new CFake(x, y, CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }
                case "invisible":
                    {
                        CElementManager.Register(new CInvisibleWall(x, y));
                        break;
                    }
                case "water":
                    {
                        CElementManager.Register(new CWater(x, y, CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }
                #endregion "Terrains"

                #region "Items"


                case "ammo":
                    {
                        CElementManager.Register(new CAmmo(x, y, int.Parse(e.Attribute("ammo").Value)));
                        break;
                    }




                #endregion "Items"

                #region "Creatures"


                case "lion":
                    {
                        CElementManager.Register(new CLion(x, y));
                        break;
                    }

                case "tiger":
                    {
                        CElementManager.Register(new CTiger(x, y));
                        break;
                    }

                case "pusher":
                    {
                        CElementManager.Register(new CPusher(x, y, (EDirection)int.Parse(e.Attribute("dir").Value), CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }

                case "shark":
                    {
                        CElementManager.Register(new CShark(x, y));
                        break;
                    }

                case "ruffian":
                    {
                        CElementManager.Register(new CRuffian(x, y));
                        break;
                    }

                case "bear":
                    {
                        CElementManager.Register(new CBear(x, y));
                        break;
                    }

                #endregion "Creatures"

                #region "Game Objects"


                case "boulder":
                    {
                        CElementManager.Register(new CBoulder(x, y, CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }

                case "slider":
                    {
                        CElementManager.Register(new CSlider(x, y, (ESliderType)int.Parse(e.Attribute("type").Value), CUtil.getColorFromString(e.Attribute("color").Value)));
                        break;
                    }


                #endregion "Game Objects"

            }

        }

        /// <summary>
        /// Loads a map, after clearing the grid of all elements.
        /// </summary>
        /// <param name="mapName">The name of the map file.</param>
        public static void LoadMap(string mapName)
        {
            string mapfolder = Path.GetDirectoryName(Application.ExecutablePath) + "\\maps";
            
            XElement root = XElement.Load(mapfolder + "\\" + mapName + ".map");
            XElement elementRoot = root.Element("elements");

            CGrid.ClearGrid();

            XElement playerTag = root.Element("player");

            //Load and create all objects
            foreach (XElement e in elementRoot.Elements())
            {
                HandleMapNode(e);
            }

            //Spawn player
            CGame.SpawnPlayer(int.Parse(playerTag.Attribute("x").Value), int.Parse(playerTag.Attribute("y").Value));

        }

    }
}
