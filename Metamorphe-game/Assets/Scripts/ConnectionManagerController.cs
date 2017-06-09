using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class ConnectionManagerController : MonoBehaviour {

    public NetworkManager manager;
    private PlayerInfoNetwork playerInfoNetwork;

    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("NetWorkManager");
        playerInfoNetwork = go.transform.GetComponent<PlayerInfoNetwork>();
        manager = go.transform.GetComponent<NetworkManager>();
    }

    private void checkPlayerName()
    {
        if (playerInfoNetwork.getPlayerName() == "")
        {
            playerInfoNetwork.setPlayerName("Anonyme");
        }
    }

    public void HostServer()
    {
        checkPlayerName();
        manager.StartHost();
    }
}
