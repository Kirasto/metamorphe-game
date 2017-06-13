using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using PlayerInfo;

public class GameController : NetworkBehaviour {

    public enum CycleState
    {
        ON_DAY,
        ON_NIGHT
    }

    public enum GameState
    {
        LOBBY_TIME,
        TALK_TIME,
        DESERTER_TIME,
        METAMORHPE_TIME,
        MEDIC_TIME
    }

    public enum PlayerRole
    {
        NONE,
        METAMORPHE,
        DESERTER,
        MEDIC
    }

    [SyncVar(hook = "OnCycleStateChanged")]
    public CycleState cycleState = CycleState.ON_DAY;
    [SyncVar(hook = "OnGameStateChanged")]
    public GameState gameState = GameState.LOBBY_TIME;
    [SerializeField]
    public EnvironmentController environmentController;

    public List<PlayerInfo.PlayerInfo> playersInfo = new List<PlayerInfo.PlayerInfo>();

    public GameObject playerObject;
    private int nextId = 0;

    private void Start()
    {
    }

    //*//   On Add Player On Server   //*//

    public int getNextId()
    {
        return nextId++;
    }

    public void addNewPlayer(PlayerInfo.PlayerInfo playerInfo)
    {
        playersInfo.Add(playerInfo);
        CmdOnPlayerConnectedFromServer(playerInfo.playerName);
    }

    //*//   Get Player Information   //*//

    public string getPlayerNameOf(int playerId)
    {
        PlayerInfo.PlayerInfo p = getPlayerInfoOf(playerId);
        if (p.isValide())
        {
            return p.playerName;
        }
        else
        {
            return "";
        }

    }

    private PlayerInfo.PlayerInfo getPlayerInfoOf(int playerId)
    {
        PlayerInfo.PlayerInfo playerInfo = new PlayerInfo.PlayerInfo();
        foreach (PlayerInfo.PlayerInfo p in playersInfo)
        {
            if (p.id == playerId)
            {
                playerInfo = p;
            }
        }
        return playerInfo;
    }

    //*//   On Player Connected Or Disconnected From Server   //*//

    [Command]
    public void CmdOnPlayerConnectedFromServer(string playerName)
    {
        CmdSendMessageToAllClient(playerName + " join the game !");
    }

    public void OnPlayerDisconnectedFromServer()
    {
        int index = 0;
        while (index < playersInfo.Count)
        {
            int id = playersInfo[index].id;
            bool flag = false;
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                if (go.GetComponent<Player>().getPlayerInfo().id == id)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                playersInfo[index].setIsDead(true);
                CmdSendMessageToAllClient(playersInfo[index].playerName + " left the game !");
            }
            index++;
        }
    }

    //*//   Send Message Function   //*//

    [Command]
    void CmdSendMessageToAllClient(string message)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            go.GetComponent<ChatManager>().CmdSendMessageToServer(message);
            break;
        }
    }

    //*//   Game Start   //*//

    [Command]
    public void CmdStartGame()
    {
        setNight();
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            go.GetComponent<Player>().RpcSetStateOfMenu(Player.MENU_TYPE.GAME);
        }
    }

    //*//   Change CycleState Function   //*//

    [Server]
    public void setNight()
    {
        cycleState = CycleState.ON_NIGHT;
    }

    [Server]
    public void setDay()
    {
        cycleState = CycleState.ON_NIGHT;
    }

    //*//   On SyncVar Change   //*//

    void OnCycleStateChanged(CycleState newCycleState)
    {
        cycleState = newCycleState;
        Debug.Log("cycle state change to: " + newCycleState.ToString());
        if (cycleState == CycleState.ON_DAY)
        {
            environmentController.setDaySkyBox();
        }
        else if (cycleState == CycleState.ON_NIGHT)
        {
            environmentController.setNightSkyBox();
        }
    }

    void OnGameStateChanged(GameState newGameState)
    {
        gameState = newGameState;
        Debug.Log("game state change");
    }
}