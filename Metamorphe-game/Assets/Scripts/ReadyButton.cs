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
        player.CmdChangeReady();
    }
}
