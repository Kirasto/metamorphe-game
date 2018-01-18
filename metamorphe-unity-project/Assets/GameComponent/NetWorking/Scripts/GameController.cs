using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace GameController
{
    public class GameController : NetworkBehaviour
    {
        CycleController cycleController;
        PlayersController playersController;

        public void Awake()
        {
            cycleController = GetComponent<CycleController>();
            playersController = GetComponent<PlayersController>();
        }

        //*//   Ready System   //*//

        [Command]
        public void CmdOnPlayerSetReady()
        {
            if (allPlayerIsReady())
            {
                Debug.Log("All player is ready");
                playersController.CmdSetRoleToPlayer();
                cycleController.CmdNextEvent();
            }
        }

        private bool allPlayerIsReady()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                if (!go.GetComponent<Player.PlayerController>().IsReady)
                {
                    return false;
                }
            }
            return true;
        }

        //*//   Vote System   //*//

        [Command]
        public void CmdOnMetamorpheVoteOnId(int playerId, int voteOnId)
        {
            playersController.CmdPlayerVote(playerId, voteOnId);
            if (playersController.isAllPlayerVote(Role.Type.metamorphe))
            {
                int voteId = playersController.getVoteId(Role.Type.metamorphe);
                if (voteId != -1)
                {
                    cycleController.CmdPlayersVotesFor(voteId);
                    cycleController.CmdNextEvent();
                }
            }
        }

        [Command]
        public void CmdOnVillagerVoteOnId(int playerId, int voteOnId)
        {
            playersController.CmdPlayerVote(playerId, voteOnId);
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            string playerName = "";
            string voteName = "";

            foreach (GameObject go in gos)
            {
                if (go.GetComponent<Player.Player>().id == playerId)
                {
                    playerName = go.GetComponent<Player.Player>().playerName;
                }
                else if (go.GetComponent<Player.Player>().id == voteOnId)
                {
                    voteName = go.GetComponent<Player.Player>().playerName;
                }
            }
            foreach (GameObject go in gos)
            {
                go.GetComponent<Player.ChatPlayerManager>().RpcRecieveMessageFromServer("[Vote] " + playerName + " as vote for: " + voteName);
            }
        }

        [Command]
        public void CmdOnTimerEndForVote()
        {
            int voteId = playersController.getVoteId();
            if (voteId != -1)
            {
                cycleController.CmdPlayersVotesFor(voteId);
            }
        }

        //*//   Win System   //*//

        [Command]
        public void CmdCheckWin()
        {
            int metaCount = 0;
            int villCount = 0;

            foreach (Player.PlayerInfo p in playersController.getPlayersInfo())
            {
                Debug.Log("playerinfo");
                if (!p.isDead)
                {
                    switch (p.role)
                    {
                        case Role.Type.metamorphe:
                            metaCount++;
                            break;
                        default:
                            villCount++;
                            break;
                    }
                }
            }
            Debug.Log("metaCount: " + metaCount);
            Debug.Log("villCount: " + villCount);
            if (metaCount == 0 && villCount > 0)
            {
                cycleController.setAnnoucement("Les villagoies on gagné");
                resetGame();
            }
            else if (villCount == 0 && metaCount > 0)
            {
                cycleController.setAnnoucement("Les métamorphes on gagné");
                resetGame();
            }
        }

        [ServerCallback]
        public void resetGame()
        {
            cycleController.CmdInitEventsList();
        }
    }
}