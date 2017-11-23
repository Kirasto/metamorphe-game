using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerInfoNetwork : MonoBehaviour
    {

        public PlayerInfo playerInfo = new PlayerInfo("Unknow");

        //*//   Get Function   //*//

        public PlayerInfo getPlayerInfo()
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
}