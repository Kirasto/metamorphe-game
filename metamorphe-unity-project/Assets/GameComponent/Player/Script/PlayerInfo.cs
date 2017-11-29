using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerInfo
    {
        public string playerName;
        public int id;
        public bool isDead;
        public bool isValid;
        public bool isReady;
        public Role.Type role;

        public bool asVote;
        public int voteOnId;

        public PlayerInfo()
        {
            playerName = "Unknow";
            id = -1;
            isDead = false;
            isReady = false;
            role = Role.Type.villager;

            asVote = false;
            voteOnId = -1;
        }

        public PlayerInfo(string name)
        {
            playerName = name;
            id = -1;
            isDead = false;
            isReady = false;
            role = Role.Type.villager;
        }

        public PlayerInfo(PlayerInfo p)
        {
            playerName = p.playerName;
            id = p.id;
            isDead = p.isDead;
            isReady = p.isReady;
            role = p.role;
        }
    }
}