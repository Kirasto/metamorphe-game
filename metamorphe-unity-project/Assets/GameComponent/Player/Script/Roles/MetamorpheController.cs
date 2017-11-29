using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    namespace Roles
    {
        public class MetamorpheController : NetworkBehaviour
        {

            public bool isOnEvent = false;
            public Camera cam;

            private GameController.GameController gameController;

            public KeyCode choiceKey;

            public void setOnEvent(bool onEvent)
            {
                isOnEvent = onEvent;
            }

            public void setGameController(GameController.GameController _gameController)
            {
                gameController = _gameController;
            }

            [Command]
            public void CmdOnChoiceVictim(int victimeiId)
            {
                Debug.Log("Game: A victim as been choise");
                gameController.CmdOnMetamorpheVoteOnId(GetComponent<Player>().id, victimeiId);
            }

            // Update is called once per frame
            [ClientCallback]
            void Update()
            {
                if (isLocalPlayer && isOnEvent)
                {
                    RaycastHit hit;
                    if (Input.GetKeyDown(choiceKey) && Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out hit))
                    {
                        if (hit.transform.gameObject.tag == "Player")
                        {
                            CmdOnChoiceVictim(hit.transform.gameObject.GetComponent<Player>().id);
                        }
                    }
                }
            }
        }
    }
}
