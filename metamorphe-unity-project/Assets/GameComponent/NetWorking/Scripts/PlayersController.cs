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

        [ServerCallback]
        public Role.Type getRoleOf(int id)
        {
            foreach (Player.PlayerInfo p in playersInfo)
            {
                if (p.id == id)
                {
                    return p.role;
                }
            }
            return Role.Type.villager;
        }

        [ServerCallback]
        public string getPlayerNameOf(int id)
        {
            foreach (Player.PlayerInfo p in playersInfo)
            {
                if (p.id == id)
                {
                    return p.playerName;
                }
            }
            return "Error";
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

        [Command]
        public void CmdSetRoleToPlayer()
        {
            int nbPlayers = playersInfo.Count;
            int nbMeta = (int)Mathf.Ceil(((float)nbPlayers) / 3.0F);
            List<Role.Type> roles = new List<Role.Type>();

            int index;
            Debug.Log("Game: " + nbMeta + " Métamorphe dans le jeu");
            while (nbMeta > 0)
            {
                index = Random.Range(0, nbPlayer);
                if (playersInfo[index].role == Role.Type.villager)
                {
                    playersInfo[index].role = Role.Type.metamorphe;
                    nbMeta--;
                }
            }
            CmdSendRoleToPlayers();
        }

        [Command]
        public void CmdSendRoleToPlayers()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                Role.Type role = getRoleOf(go.GetComponent<Player.Player>().id);
                switch (role)
                {
                    case Role.Type.metamorphe:
                        go.GetComponent<Player.Roles.MetamorpheController>().enabled = true;
                        go.GetComponent<Player.Roles.MetamorpheController>().setGameController(GetComponent<GameController>());
                        break;
                }
                go.GetComponent<Player.ChatPlayerManager>().RpcRecieveMessageFromServer((role == Role.Type.metamorphe)?("Tu es un Métamorphe"):("Tu es un Villagoie"));
                go.GetComponent<Player.PlayerController>().RpcOnReceiveRole(role);
            }
        }

        //*//   Vote System   //*//

        [Command]
        public void CmdClearVote()
        {
            int index = 0;
            while (index < playersInfo.Count)
            {
                playersInfo[index].asVote = false;
                playersInfo[index].voteOnId = -1;
                index++;
            }
        }

        [Command]
        public void CmdPlayerVote(int playerId, int voteId)
        {
            int index = playersInfo.FindIndex(obj => obj.id == playerId);
            playersInfo[index].asVote = true;
            playersInfo[index].voteOnId = voteId;
        }

        [ServerCallback]
        public bool isAllPlayerVote(Role.Type roleType)
        {
            int index = 0;
            while (index < playersInfo.Count)
            {
                if (playersInfo[index].role == roleType && !playersInfo[index].asVote)
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        [ServerCallback]
        public bool isAllPlayerVote()
        {
            int index = 0;
            while (index < playersInfo.Count)
            {
                if (!playersInfo[index].asVote)
                {
                    return false;
                }
                index++;
            }
            return true; ;
        }

        [ServerCallback]
        public int getVoteId(Role.Type roleType)
        {
            int index = 0;
            Dictionary<int, int> votesOnId = new Dictionary<int, int>();
            while (index < playersInfo.Count)
            {
                if (playersInfo[index].role == roleType && playersInfo[index].asVote)
                {
                    if (!votesOnId.ContainsKey(playersInfo[index].voteOnId))
                    {
                        votesOnId.Add(playersInfo[index].voteOnId, 1);
                    }
                    else
                    {
                        int count = 0;
                        votesOnId.TryGetValue(playersInfo[index].voteOnId, out count);
                        votesOnId.Remove(playersInfo[index].voteOnId);
                        votesOnId.Add(playersInfo[index].voteOnId, count + 1);
                    }
                }
                index++;
            }
            KeyValuePair<int, int> maxVotesOnId = new KeyValuePair<int, int>(-1, -1);
            bool isEnter = false;
            foreach (KeyValuePair<int, int> onId in votesOnId)
            {
                isEnter = true;
                if (onId.Value > maxVotesOnId.Value)
                {
                    maxVotesOnId = onId;
                }
            }
            if (isEnter)
            {
                return maxVotesOnId.Key;
            }
            return -1;
        }

        [ServerCallback]
        public int getVoteId()
        {
            int index = 0;
            Dictionary<int, int> votesOnId = new Dictionary<int, int>();
            while (index < playersInfo.Count)
            {
                if (!playersInfo[index].asVote)
                {
                    if (!votesOnId.ContainsKey(playersInfo[index].voteOnId))
                    {
                        votesOnId.Add(playersInfo[index].voteOnId, 1);
                    }
                    else
                    {
                        int count = 0;
                        votesOnId.TryGetValue(playersInfo[index].voteOnId, out count);
                        votesOnId.Remove(playersInfo[index].voteOnId);
                        votesOnId.Add(playersInfo[index].voteOnId, count + 1);
                    }
                }
                index++;
            }
            KeyValuePair<int, int> maxVotesOnId = new KeyValuePair<int, int>(-1, -1);
            bool isEnter = false;
            foreach (KeyValuePair<int, int> onId in votesOnId)
            {
                isEnter = true;
                if (onId.Value > maxVotesOnId.Value)
                {
                    maxVotesOnId = onId;
                }
            }
            if (isEnter)
            {
                return maxVotesOnId.Key;
            }
            return -1;
        }

        //*//   Death System   //*//

        [Command]
        public void CmdGiveDeathTo(int id)
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                if (go.GetComponent<Player.Player>().id == id)
                {
                    go.GetComponent<Player.PlayerController>().CmdGiveDeath();
                }
            }
        }
    }
}

namespace Role
{
    public enum Type
    {
        metamorphe,
        villager
    }
}