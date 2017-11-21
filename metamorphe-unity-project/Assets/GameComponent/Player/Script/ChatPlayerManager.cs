using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    public class ChatPlayerManager : NetworkBehaviour
    {
        private GameObject chatPanel;
        public Menu.ChatPanelController chatPanelController;

        private void Start()
        {
            chatPanel = GameObject.FindGameObjectWithTag("ChatPanel");
            chatPanelController = chatPanel.GetComponent<Menu.ChatPanelController>();
            chatPanelController.setPlayerObject();
        }

        [ClientCallback]
        public void OnNewMessageEvent(string message)
        {
            if (!isLocalPlayer)
            {
                return;
            }
            CmdSendMessageToPlayers(message);
        }

        [Command]
        protected void CmdSendMessageToPlayers(string message)
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                go.GetComponent<ChatPlayerManager>().RpcRecieveMessageFromServer(message);
            }
        }

        [ClientRpc]
        public void RpcRecieveMessageFromServer(string message)
        {
            if (isLocalPlayer)
            {
                Debug.Log(message);
                chatPanelController.addMessage(message);
            }
        }
    }
}