using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace GameController
{
    public class PlayersController : NetworkBehaviour
    {
        Player.ChatPlayerManager chatPlayerManager;
        List<Player.PlayerInfo> playersInfo;

        private int nbPlayer;
        [SyncVar]
        private int nextId;
        public int NextId
        {
            get
            {
                nextId++;
                return nextId - 1;

            }
            set
            {
                nextId = value;
            }
        }

        [Command]
        public void CmdInit()
        {
            Debug.Log("Network: Start GameController");
            playersInfo = new List<Player.PlayerInfo>();

            nbPlayer = 0;
            NextId = 0;
        }

        public void Update()
        {
            if (isServer)
            {
                CmdCheckNbPlayer();
            }
        }

        [Command]
        public void CmdCheckNbPlayer()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
            int nbCurrentPlayer = gos.Length;
            if (nbCurrentPlayer < nbPlayer)
            {
                foreach (Player.PlayerInfo pi in playersInfo)
                {
                    if (playerWithIdExist(pi.id))
                    {
                        CmdOnPlayerLeave(pi.id);
                        return;
                    }
                }
            }
            nbPlayer = nbCurrentPlayer;
        }
        
        public bool playerWithIdExist(int id)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                Player.Player player = go.GetComponent<Player.Player>();
                if (player.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        [Command]
        public void CmdOnPlayerJoin(Player.PlayerInfo _playerInfo)
        {
            Debug.Log(_playerInfo.playerName + " join with id " + _playerInfo.id);
            CmdSendMessageToPlayers("Say hello to " + _playerInfo.playerName + " ! (id: " + _playerInfo.id + ")");
            playersInfo.Add(_playerInfo);
        }

        [Command]
        public void CmdOnPlayerLeave(int id)
        {
            int index = playersInfo.FindIndex(obj => obj.id == id);
            CmdSendMessageToPlayers(playersInfo[index].playerName + " has left the game !");
            playersInfo.RemoveAt(index);
        }

        [Command]
        private void CmdSendMessageToPlayers(string message)
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                go.GetComponent<Player.ChatPlayerManager>().RpcRecieveMessageFromServer(message);
            }
        }
    }
}
