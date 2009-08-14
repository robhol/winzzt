﻿/*

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

            //Common for any element
            int x = int.Parse(e.Attribute("x").Value);
            int y = int.Parse(e.Attribute("y").Value);

            switch (e.Name.ToString())
            {
                #region "Terrains"
                case "wall":
                    {
                        new CWall(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }
                case "breakable":
                    {
                        new CBreakable(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }
                case "normal":
                    {
                        new CNormal(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }
                case "fake":
                    {
                        new CFake(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }
                case "invisible":
                    {
                        new CInvisibleWall(x, y);
                        break;
                    }
                case "water":
                    {
                        new CWater(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }
                case "forest":
                    {
                        new CForest(x, y);
                        break;
                    }
                case "door":
                    {
                        new CDoor(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }
                case "text":
                    {
                        new CText(x, y, int.Parse(e.Attribute("char").Value), CUtil.getColorFromString(e.Attribute("color").Value), CUtil.getColorFromString(e.Attribute("background").Value));
                        break;
                    }


                #endregion "Terrains"

                #region "Items"


                case "ammo":
                    {
                        new CAmmo(x, y, int.Parse(e.Attribute("ammo").Value));
                        break;
                    }
                case "key":
                    {
                        new CKey(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }



                #endregion "Items"

                #region "Creatures"


                case "lion":
                    {
                        new CLion(x, y);
                        break;
                    }

                case "tiger":
                    {
                        new CTiger(x, y);
                        break;
                    }

                case "pusher":
                    {
                        new CPusher(x, y, (EDirection)int.Parse(e.Attribute("dir").Value), CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }

                case "shark":
                    {
                        new CShark(x, y);
                        break;
                    }

                case "ruffian":
                    {
                        new CRuffian(x, y);
                        break;
                    }

                case "bear":
                    {
                        new CBear(x, y);
                        break;
                    }

                case "slime":
                    {
                        new CSlime(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }

                #endregion "Creatures"

                #region "Game Objects"


                case "boulder":
                    {
                        new CBoulder(x, y, CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }

                case "slider":
                    {
                        new CSlider(x, y, (ESliderType)int.Parse(e.Attribute("type").Value), CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }

                case "conveyor":
                    {
                        new CConveyor(x, y, (EConveyorType)int.Parse(e.Attribute("type").Value), CUtil.getColorFromString(e.Attribute("color").Value));
                        break;
                    }


                #endregion "Game Objects"

            }

        }

        /// <summary>
        /// Loads a map from an XML node, after clearing the grid of all elements.
        /// </summary>
        /// <param name="root">The XML root node.</param>
        public static void ProcessXMLMap(XElement root)
        {

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

            //Check for initial ammo setting and apply
            XAttribute ammo = playerTag.Attribute("ammo");
            if (ammo != null)
                CGame.PlayerAmmo = int.Parse(ammo.Value);

        }

        /// <summary>
        /// Loads a map with the given name, after clearing the grid of all elements.
        /// </summary>
        /// <param name="mapName">The name of the map file.</param>
        public static void LoadMapFile(string mapName)
        {
            string mapfolder = Path.GetDirectoryName(Application.ExecutablePath) + "\\maps";

            if (!Directory.Exists(mapfolder))
            {
                if (MessageBox.Show("Couldn't find /maps/ folder. This should be in the same directory as this executable.", "Couldn't find maps", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    Application.Exit(); // Terminate on Cancel

            }

            string filepath = mapfolder + "\\" + mapName + ".map";

            if (!File.Exists(filepath))
            {
                if (MessageBox.Show("Couldn't find map file. This should be in the /maps/ folder and have a .map extension.", "Couldn't find map", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    Application.Exit(); // Again.. terminate on Cancel
            }
            else
            {
                ProcessXMLMap(XElement.Load(filepath));
                return; //The map is loaded; we're done here.
            }

            //If we're here, map loading failed and our user still wants to continue.
            //We'll load a default map.
            ProcessXMLMap(XElement.Parse(Properties.Resources.defaultmap));

        }

    }
}
