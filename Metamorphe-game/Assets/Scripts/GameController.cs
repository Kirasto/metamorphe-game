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
    
    public List<PlayerInfo.PlayerInfo> playersInfo = new List<PlayerInfo.PlayerInfo>();

    public GameObject playerObject;
    private int nextId = 0;

    private void Start()
    {
    }

    public int getNextId()
    {
        return nextId++;
    }

    public void addNewPlayer(PlayerInfo.PlayerInfo playerInfo)
    {
        playersInfo.Add(playerInfo);
        CmdOnPlayerConnectedFromServer(playerInfo.playerName);
    }

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

    void OnCycleStateChanged(CycleState newCycleState)
    {
        cycleState = newCycleState;
        Debug.Log("cycle state change");
    }

    void OnGameStateChanged(GameState newGameState)
    {
        gameState = newGameState;
        transform.Find("Text").GetComponent<Text>().text = "2";
        Debug.Log("game state change");
    }
}