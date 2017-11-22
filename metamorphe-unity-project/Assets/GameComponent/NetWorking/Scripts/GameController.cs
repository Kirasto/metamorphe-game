using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace GameController
{
    public class GameController : NetworkBehaviour
    {
        [Command]
        public void CmdOnPlayerSetReady()
        {
            if (allPlayerIsReady())
            {
                Debug.Log("All player is ready");
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