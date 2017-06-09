using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace PlayerInfo
{
    public struct PlayerInfo
    {

        public string playerName;
        public int id;
        public GameController.PlayerRole role;
        public bool isDead;

        public PlayerInfo(string name)
        {
            playerName = name;
            id = -1;
            role = GameController.PlayerRole.NONE;
            isDead = false;
        }

        public bool isValide()
        {
            if (isDead)
            {
                return false;
            }
            return true;
        }

        public void setIsDead(bool _isDead)
        {
            isDead = _isDead;
        }
    }
}