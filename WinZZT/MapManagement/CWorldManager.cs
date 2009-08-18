using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Drawing;
using System.Windows.Forms;


namespace WinZZT
{
    static class CWorldManager
    {

        public static bool InitialSpawn = true;
        static Dictionary<string, CMap> MapList = new Dictionary<string, CMap>();

        public static string WorldName;
        public static string Author;

        public static void LoadWorld(string filename)
        {

            InitialSpawn = true;

            string folder = Path.GetDirectoryName(Application.ExecutablePath) + "\\worlds";

            if (!Directory.Exists(folder))
            {
                if (MessageBox.Show("Couldn't find /worlds/ folder. This should be in the same directory as this executable.", "Couldn't find worlds", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    Application.Exit();
                else
                    //Load a default screen
                    CMapManager.ProcessXMLMap(XElement.Parse(Properties.Resources.defaultmap), true, true);

                return;
            }

            string filepath = folder + "\\" + filename + ".wzw";

            XElement root = XElement.Load(filepath);

            WorldName = root.Attribute("name").Value;
            Author = root.Attribute("author").Value;

            foreach (XElement w in root.Elements("winzztmap"))
            {
                CMap m = new CMap(w);
                MapList.Add(m.Name, m);
            }

            getMap(root.Attribute("startmap").Value).ChangeTo(true);

        }

        public static CMap getMap(string name)
        {

            if (!MapList.ContainsKey(name))
                return null;

            return MapList[name];

        }

        public static void ChangeMap(string name)
        {
            getMap(name).ChangeTo(false);
        }

        public static void ChangeMap(string name, Point p)
        {
            getMap(name).ChangeTo(p, false);
        }


    }
}
