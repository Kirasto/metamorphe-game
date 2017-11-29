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
                }
            }
        }

        [Command]
        public void CmdOnTimerEndForVote()
        {
            int voteId = playersController.getVoteId(Role.Type.metamorphe);
            if (voteId != -1)
            {
                cycleController.CmdPlayersVotesFor(voteId);
            }
        }

        //*//   Win System   //*//

        [Command]
        public void CmdCheckWin()
        {
            Debug.Log("Game: On check win");
        }
    }
}