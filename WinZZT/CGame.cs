using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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



    }
}
