using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Player
{
    public class PlayerStart : NetworkBehaviour
    {
        public GameObject gameControllerPrefabs;
        private GameController.PlayersController playersController;
        private GameController.CycleController cycleController;
        private GameController.TimerController timerController;

        // Use this for initialization
        void Start()
        {
            if (isServer)
            {
                playersController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.PlayersController>();
                cycleController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.CycleController>();
                timerController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.TimerController>();
            }
            if (isLocalPlayer && isServer)
            {
                playersController.CmdInit();
                cycleController.CmdInitEventsList();
                timerController.CmdInit();
            }
            if (isLocalPlayer)
            {
                transform.Find("FirstPersonCharacter").gameObject.SetActive(true);
                GetComponent<CharacterController>().enabled = true;
                GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
                InitSpawn();
                InitPlayerInfo();
            }
        }

        [Command]
        public void CmdCreateGameController()
        { 
        }

        void InitSpawn()
        {
            GameObject[] globalSpawns;
            int spawnId;

            globalSpawns = GameObject.FindGameObjectsWithTag("GlobalSpawn");
            spawnId = Random.Range(0, globalSpawns.Length);
            transform.position = globalSpawns[spawnId].transform.position;
            transform.rotation = globalSpawns[spawnId].transform.rotation;
        }

        void InitPlayerInfo()
        {
            if (!isLocalPlayer) { return; }
            PlayerInfo playerInfo = GameObject.FindGameObjectWithTag("GameInfo").GetComponent<PlayerInfoNetwork>().getPlayerInfo();
            CmdSendPlayerInfoToServer(playerInfo);
        }

        [Command]
        public void CmdSendPlayerInfoToServer(PlayerInfo playerInfo)
        {
            playerInfo.id = playersController.NextId;
            GetComponent<Player>().CmdInitPlayer(playerInfo);
            playersController.CmdOnPlayerJoin(playerInfo);
        }
    }
}