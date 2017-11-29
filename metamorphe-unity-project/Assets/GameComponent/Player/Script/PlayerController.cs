using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        private GameController.GameController gameController;
        private GameController.CycleController cycleController;
        private Menu.Timer.TimerPanelController timerPanelController;
        private Menu.Effect.EffectPanelContoller effectPanelContoller;
        private ChatPlayerManager chatPlayerManager;
        private Player player;

        public KeyCode readyKey;
        public Role.Type roleType;

        public void Start()
        {
            if (isServer)
            {
                gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.GameController>();
                cycleController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.CycleController>();
            }
            timerPanelController = GameObject.FindGameObjectWithTag("TimerPanel").GetComponent<Menu.Timer.TimerPanelController>();
            effectPanelContoller = GameObject.FindGameObjectWithTag("EffectPanel").GetComponent<Menu.Effect.EffectPanelContoller>();
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
            if (Input.GetKeyDown(KeyCode.T))
            {
                CmdGiveDeath();
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
            
        public void setControlToPlayer(bool canControl, bool onBlind = true)
        {
            GetComponent<CharacterController>().enabled = canControl;
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = canControl;
            effectPanelContoller.setBlindEffect((onBlind && !canControl)?true:false);
        }

        [ClientRpc]
        public void RpcOnSetTimer(int sec)
        {
            if (isLocalPlayer)
            {
                timerPanelController.setTimer(sec);
            }
        }

        [ClientRpc]
        public void RpcOnReceiveRole(Role.Type _roleType)
        {
            roleType = _roleType;
            switch (roleType)
            {
                case Role.Type.metamorphe:
                    GetComponent<Roles.MetamorpheController>().enabled = true;
                    break;
            }
        }

        //*//   Death System   //*//

        [Command]
        public void CmdGiveDeath()
        {
            GetComponent<Player>().isDead = true;
            RpcReceiveDeath();
        }

        [ClientRpc]
        public void RpcOnPlayerDeath(int id)
        {
            if (!isLocalPlayer || id == GetComponent<Player>().id) { return; }
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                if (go.GetComponent<Player>().id == id)
                {
                    if (!GetComponent<Player>().isDead)
                    {
                        go.transform.Find("Body").gameObject.SetActive(false);
                    }
                }
            }
        }

        [ClientRpc]
        public void RpcReceiveDeath()
        {
            GameObject body = transform.Find("Body").gameObject;
            if (!isLocalPlayer) {
                return;
            }
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                if (go.GetComponent<Player>().isDead)
                {
                    go.transform.Find("Body").gameObject.SetActive(true);
                }
            }
            CmdOnDeathComplete();
        }

        [Command]
        public void CmdOnDeathComplete()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                go.GetComponent<PlayerController>().RpcOnPlayerDeath(GetComponent<Player>().id);
            }
        }
    }
}
