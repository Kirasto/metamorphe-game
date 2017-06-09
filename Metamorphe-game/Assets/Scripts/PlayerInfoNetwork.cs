using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInfo;

public class PlayerInfoNetwork : MonoBehaviour {

    private PlayerInfo.PlayerInfo playerInfo = new PlayerInfo.PlayerInfo("Unknow");

    //*//   Get Function   //*//

    public PlayerInfo.PlayerInfo getPlayerInfo()
    {
        return playerInfo;
    }

    //*//   Set Function   //*//

    public string getPlayerName()
    {
        return playerInfo.playerName;
    }

    public void setPlayerName(string playerName)
    {
        playerInfo.playerName = playerName;
    }
}