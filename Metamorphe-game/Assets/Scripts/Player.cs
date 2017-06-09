using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using PlayerInfo;

public class Player : NetworkBehaviour {

    //*//   Public Variable   //*//

    public ChatManager chatManager;
    public GameController gameController;

    //*//   Private Variable   //*//

    [SyncVar]
    private PlayerInfo.PlayerInfo playerInfo;

    //*//   Default Function   //*//

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<GameController>();
        if (isLocalPlayer)
        {
            PlayerInfo.PlayerInfo _playerInfo = GameObject.FindGameObjectWithTag("NetWorkManager").transform.GetComponent<PlayerInfoNetwork>().getPlayerInfo();
            CmdOnPlayerConnected(_playerInfo);
        }
    }

    //*//   On Player Change   //*//

    [Command]
    void CmdOnPlayerConnected(PlayerInfo.PlayerInfo _playerInfo)
    {
        playerInfo = _playerInfo;
        playerInfo.id = gameController.getNextId();
        gameController.addNewPlayer(playerInfo);
    }

    //*//   Get Function   //*//

    public string getPlayerName()
    {
        return playerInfo.playerName;
    }

    public PlayerInfo.PlayerInfo getPlayerInfo()
    {
        return playerInfo;
    }

    //*//   Is Function   //*//

    public bool isLocalObject()
    {
        if (isLocalPlayer)
        {
            return true;
        }
        return false;
    }
}
