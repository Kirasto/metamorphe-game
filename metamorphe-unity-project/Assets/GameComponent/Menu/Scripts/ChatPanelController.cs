using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Menu
{

    public class ChatPanelController : NetworkBehaviour
    {
        public TMPro.TMP_InputField chatInputField;
        public GameObject messagePrefab;
        public GameObject messagesListPanel;

        private GameObject playerObject;
        private Player.ChatPlayerManager chatPlayerManager;

        private void Start()
        {
        }

        public void setPlayerObject()
        {
            GameObject[] playersObject = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in playersObject)
            {
                if (player.GetComponent<Player.PlayerStart>().isLocalPlayer)
                {
                    playerObject = player;
                    chatPlayerManager = playerObject.GetComponent<Player.ChatPlayerManager>();
                    return;
                }
            }
        }

        public void refreshChat()
        {
            return;
        }

        private string getMessageToSend()
        {
            return chatInputField.text;
        }

        public void OnSendMessageEvent()
        {
            Debug.Log("Message Send");
            chatPlayerManager.OnNewMessageEvent(getMessageToSend());
        }

        public void addMessage(string message)
        {
            chatInputField.text = "";

            GameObject messageObject = Instantiate(messagePrefab, messagesListPanel.transform);
            messageObject.transform.SetParent(messagesListPanel.transform, false);
            messageObject.GetComponent<TMPro.TMP_Text>().SetText(message);
            return;
        }
    }
}