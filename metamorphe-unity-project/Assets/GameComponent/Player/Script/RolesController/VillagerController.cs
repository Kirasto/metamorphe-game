using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Player
{
    namespace RolesController
    {
        public class VillagerController : NetworkBehaviour
        {
            public bool isOnEvent = false;
            public Camera cam;

            private GameController.GameController gameController;

            public KeyCode choiceKey;

            public void setOnEvent(bool onEvent)
            {
                isOnEvent = onEvent;
            }

            public void Start()
            {
                gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController.GameController>(); ;
            }

            [Command]
            public void CmdOnVote(int voteId)
            {
                gameController.CmdOnVillagerVoteOnId(GetComponent<Player>().id, voteId);
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
                            CmdOnVote(hit.transform.gameObject.GetComponent<Player>().id);
                        }
                    }
                }
            }
        }
    }
}