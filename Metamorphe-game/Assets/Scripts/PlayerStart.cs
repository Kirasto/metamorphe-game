using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStart : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            transform.Find("FirstPersonCharacter").gameObject.SetActive(true);
            transform.Find("Canvas").gameObject.SetActive(true);
        }
	}
}