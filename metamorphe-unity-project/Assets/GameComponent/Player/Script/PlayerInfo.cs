using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public struct PlayerInfo
    {
        public string playerName;
        public int id;
        public bool isDead;
        public bool isReady;
        public Role.Type roleType;

        public PlayerInfo(string name)
        {
            playerName = name;
            id = -1;
            isDead = false;
            isReady = false;
            roleType = unknow;
        }

        public PlayerInfo(PlayerInfo p)
        {
            playerName = p.playerName;
            id = p.id;
            isDead = p.isDead;
            isReady = p.isReady;
            roleType = p.roleType;
        }
    }
}