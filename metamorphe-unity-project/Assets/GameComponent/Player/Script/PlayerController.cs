﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        private GameController.GameController gameController;
        private GameController.CycleController cycleController;
        private ChatPlayerManager chatPlayerManager;
        private Player player;

        public KeyCode readyKey;

        public void Start()
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.GameController>();
            cycleController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.CycleController>();
            chatPlayerManager = GetComponent<ChatPlayerManager>();
            player = GetComponent<Player>();

            readyKey = KeyCode.R;
        }

        public void Update()
        {
            if (!isLocalPlayer) { return; }
            if (Input.GetKeyDown(readyKey))
            {
                switchReady();
            }
        }

        [SyncVar]
        private bool isReady;
        public bool IsReady
        {
            get { return isReady; }
        }

        public void switchReady()
        {
            CmdSwitchReady();
        }

        [Command]
        public void CmdSwitchReady()
        {
            if (cycleController.timeOf != GameController.CycleController.TimeOf.wait) { return; }
            isReady = !isReady;

            string newMessage;
            newMessage = "[" + player.playerName + "]" + ((IsReady)?(" is ready !"):("is not ready !"));
            chatPlayerManager.CmdSendMessageToPlayers(newMessage);
            gameController.CmdOnPlayerSetReady();
        }
            
        public void setControlToPlayer(bool canControl)
        {
            GetComponent<CharacterController>().enabled = canControl;
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = canControl;
        }
    }
}
