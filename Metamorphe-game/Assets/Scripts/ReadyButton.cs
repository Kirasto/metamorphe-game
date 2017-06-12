using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour {
    [SerializeField]
    public ChatManager chatManager;
    [SerializeField]
    public Player player;

    public void ClickReadyButton()
    {
        bool isReady = !player.getPlayerInfo().isReady;
        player.CmdOnPlayerChangeReady(isReady);
        if (isReady)
        {
            chatManager.CmdSendMessageToServer(player.getPlayerName() + " is now ready !");
        }
        else
        {
            chatManager.CmdSendMessageToServer(player.getPlayerName() + " is not ready !");
        }
    }
}
