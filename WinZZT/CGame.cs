using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinZZT
{
    static class CGame
    {
        static private CPlayer _player;
        static private int _playerHealth;

        static public bool PlayerSpawned;
        static public bool PlayerDead;
        static public bool PlayerFrozen;

        static public int PlayerAmmo = 0;

        static public CPlayer Player
        {
            get { return _player; }
        }

        static public int PlayerHealth
        {
            get { return _playerHealth; }
        }

        static public void DamagePlayer(int dmg)
        {
            _playerHealth = Math.Max(_playerHealth - dmg, 0);
            if (_playerHealth == 0)
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
            _playerHealth = 100;
            _player = new CPlayer(x, y);

        }



    }
}
