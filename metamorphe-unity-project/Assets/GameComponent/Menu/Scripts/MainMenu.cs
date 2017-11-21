using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Menu
{

    public class MainMenu : MonoBehaviour
    {

        public NetworkManager manager;
        private Player.PlayerInfoNetwork playerInfoNetwork;

        // Panels GameObject

        public GameObject mainMenuPanel;
        public GameObject chatPanel;
        public GameObject messagePanel;

        // UI Items

        public TMPro.TMP_InputField nameInputField;
        public TMPro.TMP_InputField portInputField;
        public TMPro.TMP_InputField ipInputField;

        void Start()
        {
            GameObject go = GameObject.FindGameObjectWithTag("GameInfo").transform.Find("NetworkManager").gameObject;
            playerInfoNetwork = GameObject.FindGameObjectWithTag("GameInfo").GetComponent<Player.PlayerInfoNetwork>();
            manager = go.transform.GetComponent<NetworkManager>();
        }

        private void Update()
        {
            if (NetworkClient.active && mainMenuPanel.activeSelf)
            {
                mainMenuPanel.SetActive(false);
            }
            else if (!NetworkClient.active && !mainMenuPanel.activeSelf)
            {
                mainMenuPanel.SetActive(true);
            }

            if (!NetworkClient.active && chatPanel.activeSelf)
            {
                chatPanel.SetActive(false);
            }
            else if (NetworkClient.active && !chatPanel.activeSelf)
            {
                chatPanel.SetActive(true);
            }
        }

        private void checkPlayerName()
        {
            if (playerInfoNetwork.getPlayerName() == "")
            {
                playerInfoNetwork.setPlayerName("Anonyme");
            }
        }

        // NetWorking Function

        public void updateName(string _name)
        {
            string name = nameInputField.text;

            playerInfoNetwork.setPlayerName(name);
            Debug.Log("Player name update to: " + name);
        }

        public void HostServer()
        {
            Debug.Log("Host game on port: " + manager.networkPort);
            checkPlayerName();
            manager.networkPort = 7777;
            manager.StartHost();
        }

        public void JoinGame()
        {
            manager.networkPort = 7777;
            manager.StartClient();
        }

        // Get functions

        GameObject getMainMenuPanel()
        {
            return mainMenuPanel;
        }

        GameObject getChatPanel()
        {
            return chatPanel;
        }

        GameObject getMessagePanel()
        {
            return messagePanel;
        }
    }
}