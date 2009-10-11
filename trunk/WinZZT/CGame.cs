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
using System.Timers;

namespace WinZZT
{
    static class CGame
    {
        static private CPlayer _player;
        static private int _TorchTime = 0;
        static public int PlayerHealth;

        static public bool PlayerSpawned;
        static public bool PlayerDead;
        static public bool PlayerFrozen;

        static public int PlayerAmmo = 0;
        static public int PlayerTorches = 0;
        static public int PlayerGems = 0;
        static public int PlayerScore = 0;

        static public EGamemode GameMode = EGamemode.Game;

        static public Random Random = new Random();
        static private Timer GameTimer = new Timer(1000);

        static public Dictionary<Color, int> KeyList = new Dictionary<Color, int>();

        static public CPlayer Player
        {
            get { return _player; }
        }

        static public bool TorchActive
        {
            get { return _TorchTime > 0; }
        }

        static public int TorchTime
        {
            get { return _TorchTime; }
        }

        static public void Initialize()
        {
            GameTimer.Elapsed += new ElapsedEventHandler(GameTimer_Elapsed);
            GameTimer.Start();
        }

        static void GameTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Count down torch time
            if (_TorchTime > 0)
                _TorchTime--;
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

        static public void SpawnPlayer(int x, int y, bool initial)
        {

            PlayerDead = false;
            PlayerSpawned = true;

            if (initial)
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

        static public void UseTorch()
        {
            if (PlayerTorches == 0)
            {
                CDrawing.DisplayText("No torches!", 1000);
                return;
            }

            PlayerTorches--;
            _TorchTime += 22;

        }

    }
}
