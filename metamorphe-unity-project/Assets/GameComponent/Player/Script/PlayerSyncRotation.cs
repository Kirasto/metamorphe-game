using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerSyncRotation : NetworkBehaviour
{

    [SyncVar]
    private Quaternion syncRot;

    [SerializeField]
    Transform myTransform;

    [SerializeField]
    float lerpRate = 15;

    void FixedUpdate()
    {
        TransmitRotation();
        LerpRotation();
    }

    void LerpRotation()
    {
        if (!isLocalPlayer)
        {
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer(Quaternion rot)
    {
        syncRot = rot;
    }

    [ClientCallback]
    void TransmitRotation()
    {
        if (isLocalPlayer)
        {
            CmdProvidePositionToServer(myTransform.rotation);
        }
    }
}
