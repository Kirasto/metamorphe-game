using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    public class Player : NetworkBehaviour
    {
        [SyncVar]
        public int id;
        [SyncVar]
        public string playerName;

        [Command]
        public void CmdInitPlayer(PlayerInfo _playerInfo)
        {
            id = _playerInfo.id;
            playerName = _playerInfo.playerName;
        }
    }
}