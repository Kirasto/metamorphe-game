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
    public MENU_TYPE menuType;
    [SyncVar]
    private PlayerInfo.PlayerInfo playerInfo = new PlayerInfo.PlayerInfo();

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
        _playerInfo.id = gameController.getNextId();
        playerInfo = new PlayerInfo.PlayerInfo(_playerInfo);
        gameController.addNewPlayer(_playerInfo);
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
            case MENU_TYPE.GAME:
                GetComponent<CharacterController>().enabled = true;
                GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
                break;
        }
        CmdChangeMenuType(nextMenuType);
    }

    [Command]
    private void CmdChangeMenuType(MENU_TYPE _menuType)
    {
        menuType = _menuType;
    }

    [Command]
    public void CmdChangeReady()
    {
        bool isReady = !playerInfo.isReady;
        CmdSetReady(isReady);
        if (isReady)
        {
            chatManager.CmdSendMessageToServer(getPlayerName() + " is now ready !");
        }
        else
        {
            chatManager.CmdSendMessageToServer(getPlayerName() + " is not ready !");
        }
    }

    //*//   Get Function   //*//

    public string getPlayerName()
    {
        return playerInfo.playerName;
    }

    public int getId()
    {
        return playerInfo.id;
    }

    //*//   Set Function   //*//

    [Command]
    public void CmdSetReady(bool isReady)
    {
        playerInfo.setReady(isReady);
        gameController.CmdOnPlayerChangeReady(playerInfo.id, isReady);
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
