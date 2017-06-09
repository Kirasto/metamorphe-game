using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using PlayerInfo;

public class ChatManager : NetworkBehaviour {

    [SyncVar]
    public SyncListString chatMessages = new SyncListString();
    
    public NetworkPlayer networkPlayer;

    [SerializeField]
    public TMP_Text textChat;

    [SerializeField]
    public TMP_InputField textField;

    [SerializeField]
    GameObject playerInfoNetwork;

    GameObject gameManager;
    public Player player;

    [ClientCallback]
    public void OnCheckForNewMessage()
    {
        if (textField.text[textField.text.Length] == '\n')
        {
            OnNewMessageEvent();
        }
    } 

    [ClientCallback]
    public void OnNewMessageEvent ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (textField.text != "")
        {
            CmdSendMessageToServer(player.getPlayerName() + ": " + textField.text);
            textField.text = "";
        }
    }

    [Command]
    public void CmdSendMessageToServer(string message)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            go.GetComponent<ChatManager>().RpcSendMessageToClients(message);
        }
    }

    [ClientRpc]
    void RpcSendMessageToClients(string message)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        chatMessages.Add(message);
        UpdateChat();
    }

    void UpdateChat()
    {
        int i = 0;
        string newTextChat = "";
        foreach (string c in chatMessages)
        {
            string newC = "";
            if (i != 0)
            {
                newC = "\n";
            }
            newC += c;
            i += 1;
            newTextChat += newC;
        }
        textChat.text = newTextChat;
    }
}