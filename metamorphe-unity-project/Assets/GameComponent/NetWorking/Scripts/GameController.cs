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
    }
}