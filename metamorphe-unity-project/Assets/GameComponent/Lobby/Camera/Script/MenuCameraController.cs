using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour {

    public int rotateSpeed = 1;
    public Transform targetTransform;

    private void Start()
    {
        transform.LookAt(targetTransform);
    }

    private void Update()
    {
        transform.RotateAround(new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z), new Vector3(0.0f, 1.0f, 0.0f), Time.deltaTime * rotateSpeed);
    }
}
