using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using PlayerInfo;

public class Player : NetworkBehaviour {

    public enum MENU_TYPE
    {
        LOBBY,
        GAME,
        PAPER,
        END
    }

    //*//   Public Variable   //*//

    public ChatManager chatManager;
    public GameController gameController;

    //*//   Private Variable   //*//

    [SyncVar]
    private PlayerInfo.PlayerInfo playerInfo;
    [SyncVar]
    private MENU_TYPE menuType;

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

    [Command]
    public void CmdOnPlayerChangeReady(bool isReady)
    {
        playerInfo.setReady(isReady);
        if (isAllPlayerReady())
        {
            gameController.CmdStartGame();
        }
    }

    [ClientRpc]
    public void RpcSetStateOfMenu(MENU_TYPE nextMenuType)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        switch (menuType)
        {
            case MENU_TYPE.LOBBY:
                transform.Find("Canvas").gameObject.transform.Find("LobbyMenu").gameObject.SetActive(false);
                break;
        }
        switch (nextMenuType)
        {
            case MENU_TYPE.LOBBY:
                transform.Find("Canvas").gameObject.transform.Find("LobbyMenu").gameObject.SetActive(true);
                break;
        }
        CmdChangeMenuType(nextMenuType);
    }

    [Command]
    private void CmdChangeMenuType(MENU_TYPE _menuType)
    {
        menuType = _menuType;
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

    private bool isAllPlayerReady()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            if (!go.GetComponent<Player>().getPlayerInfo().isReady)
            {
                return false;
            }
        }
        return true;
    }

    public bool isLocalObject()
    {
        if (isLocalPlayer)
        {
            return true;
        }
        return false;
    }
}
