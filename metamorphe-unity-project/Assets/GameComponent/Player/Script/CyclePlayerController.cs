using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    public class CyclePlayerController : NetworkBehaviour
    {
        private Menu.Annoucement.AnnouncementPanelController announcementPanelController;
        private PlayerController playerController;


        public Material nightSkyboxMaterial;
        public Material daySkyboxMaterial;
        public Skybox skybox;

        GameController.CycleController.TimeOf timeOf;

        public void Start()
        {
            announcementPanelController = GameObject.FindGameObjectWithTag("AnnouncementPanel").GetComponent<Menu.Annoucement.AnnouncementPanelController>();
            playerController = GetComponent<PlayerController>();
            if (isLocalPlayer)
            {
                announcementPanelController.setTitleAnnoucement("Bienvenue dans la partie");
            }
        }

        [ClientRpc]
        public void RpcOnDayCycleChange(GameController.CycleController.DayCycle dayCycle)
        {
            if (!isLocalPlayer) { return; }
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
        public void RpcOnEventTimeChange(GameController.CycleController.TimeOf _timeOf)
        {
            bool canControl;
            Role.Type roleType = playerController.roleType;

            if (!isLocalPlayer) { return; }
            switch (timeOf)
            {
                case GameController.CycleController.TimeOf.wait:
                    break;
                case GameController.CycleController.TimeOf.seeRole:
                    playerController.setControlToPlayer(true);
                    break;
                case GameController.CycleController.TimeOf.metamorphe:
                    playerController.setControlToPlayer(true);

                    if (roleType == Role.Type.metamorphe)
                    {
                        GetComponent<Roles.MetamorpheController>().setOnEvent(false);
                    }
                    break;
                case GameController.CycleController.TimeOf.vote:
                    break;
                default:
                    break;
            }
            timeOf = _timeOf;
            switch (timeOf)
            {
                case GameController.CycleController.TimeOf.wait:
                    break;
                case GameController.CycleController.TimeOf.seeRole:
                    announcementPanelController.setTitleAnnoucement("Vous ête un " + playerController.roleType, 10);
                    playerController.setControlToPlayer(false);
                    break;
                case GameController.CycleController.TimeOf.metamorphe:
                    announcementPanelController.setTitleAnnoucement("C'est l'heure des Métamorphes");
                    canControl = (roleType == Role.Type.metamorphe) ? true : false;
                    playerController.setControlToPlayer(canControl);

                    if (roleType == Role.Type.metamorphe)
                    {
                        GetComponent<Roles.MetamorpheController>().setOnEvent(true);
                    }
                    break;
                case GameController.CycleController.TimeOf.vote:
                    announcementPanelController.setTitleAnnoucement("C'est parti pour le vote");
                    break;
                default:
                    break;
            }
        }

        [ClientRpc]
        public void RpcOnReceivePlayerDeath(string name)
        {
            if (!isLocalPlayer) { return; }
            announcementPanelController.setTitleAnnoucement(name + " est mort", 3);
        }
    }
}