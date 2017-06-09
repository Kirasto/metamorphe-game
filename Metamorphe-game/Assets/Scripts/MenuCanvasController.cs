using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuCanvasController : MonoBehaviour {

    public TMP_InputField playerNameField;

    public void OnChangePlayerName()
    {
        GameObject go = GameObject.FindGameObjectWithTag("NetWorkManager");
        go.transform.GetComponent<PlayerInfoNetwork>().setPlayerName(playerNameField.text);
    }
}
