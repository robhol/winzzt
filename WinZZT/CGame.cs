/*

CGame.cs

Things pertaining to the global game state. 
Includes player damage handling, health and ammo
as well as spawned, dead, and frozen flags.
  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinZZT
{
    static class CGame
    {
        static public CPlayer _player;
        static public int PlayerHealth;

        static public bool PlayerSpawned;
        static public bool PlayerDead;
        static public bool PlayerFrozen;

        static public int PlayerAmmo = 0;

        static public Random Random = new Random();

        static public Dictionary<Color, int> KeyList = new Dictionary<Color, int>();

        static public CPlayer Player
        {
            get { return _player; }
        }

        static public void DamagePlayer(int dmg)
        {
            PlayerHealth = Math.Max(PlayerHealth - dmg, 0);
            if (PlayerHealth == 0)
            {
                PlayerDead = true;
                PlayerFrozen = true;
            }
        }

        static public void SpawnPlayer(int x, int y)
        {
            if (PlayerSpawned && !PlayerDead)
                return;

            PlayerDead = false;
            PlayerSpawned = true;
            PlayerHealth = 100;
            _player = new CPlayer(x, y);

        }

        static public bool AddKey(Color c)
        {

            if (KeyList.Count == 8)
                return false;

            if (!KeyList.ContainsKey(c))
                KeyList[c] = 1;
            else
                KeyList[c]++;

            return true;
        }

        static public bool UseKey(Color c)
        {

            if (!KeyList.ContainsKey(c))
                return false;

            if (KeyList[c] == 0)
                return false;

            if (--KeyList[c] == 0) // Decrement. If no more keys, remove entry.
                KeyList.Remove(c);

            return true;

        }

    }
}
