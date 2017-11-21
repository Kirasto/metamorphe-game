using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

    // Just to save object on load

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
