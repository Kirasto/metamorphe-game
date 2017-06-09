using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckConnection : NetworkBehaviour {
	private int previousCheckPlayerConnected = 1;
    public GameController gameController;

	// Update is called once per frame
	void Update () {
		if (isServer)
        {
            CmdCheckConnection();
        }
	}

    [Command]
    void CmdCheckConnection() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        if (gos.Length < previousCheckPlayerConnected)
        {
            gameController.OnPlayerDisconnectedFromServer();
        }
        previousCheckPlayerConnected = gos.Length;
    }
}
