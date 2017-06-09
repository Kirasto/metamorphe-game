using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour {

    public int rotateSpeed = 1;
    public Transform target;

    private void Start()
    {
        transform.LookAt(target);
    }

    void Update () {
        transform.RotateAround(target.position, new Vector3(0.0f, 1.0f, 0.0f), Time.deltaTime * rotateSpeed);
    }
}
