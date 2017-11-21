using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Player_ : NetworkBehaviour {

    public enum MENU_TYPE
    {
        LOBBY,
        GAME,
        PAPER,
        END
    }

    //*//   Public Variable   //*//

    //public GameController gameController;

    //*//   Private Variable   //*//

    [SyncVar]
    public MENU_TYPE menuType;

    //*//   Default Function   //*//

    private void Start()
    {
        //gameController = GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<GameController>();
        if (isLocalPlayer)
        {
            //PlayerInfo.PlayerInfo _playerInfo = GameObject.FindGameObjectWithTag("NetWorkManager").transform.GetComponent<PlayerInfoNetwork>().getPlayerInfo();
        }
    }

    //*//   On Player Change   //*//

    /*[ClientRpc]
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
    }*/

    //*//   Get Function   //*//


    //*//   Set Function   //*//


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
