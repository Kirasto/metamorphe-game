using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    public class CyclePlayerController : NetworkBehaviour
    {
        private GameController.CycleController cycleController;
        private PlayerController playerController;


        public Material nightSkyboxMaterial;
        public Material daySkyboxMaterial;
        public Skybox skybox;

        public void Start()
        {
            //cycleController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.CycleController>();
            playerController = GetComponent<PlayerController>();
        }

        [ClientRpc]
        public void RpcOnDayCycleChange(GameController.CycleController.DayCycle dayCycle)
        {
            switch (dayCycle)
            {
                case GameController.CycleController.DayCycle.day:
                    skybox.material = daySkyboxMaterial;
                    break;
                case GameController.CycleController.DayCycle.night:
                    skybox.material = nightSkyboxMaterial;
                    break;
            }
        }

        [ClientRpc]
        public void RpcOnEventTimeChange(GameController.CycleController.TimeOf timeOf)
        {
            switch (timeOf)
            {
                case GameController.CycleController.TimeOf.wait:
                    break;
                default:
                    playerController.setControlToPlayer(false);
                    break;
            }
        }
    }
}