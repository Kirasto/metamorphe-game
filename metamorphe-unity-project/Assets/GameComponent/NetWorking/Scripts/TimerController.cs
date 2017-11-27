using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace GameController
{
    public class TimerController : NetworkBehaviour
    {

        public float timer;
        private bool isTimerOn;

        CycleController cycleController;

        // Update is called once per frame
        [ServerCallback]
        void Update()
        {
            if (isServer && isTimerOn)
            {
                CmdUpdateTimer();
            }
        }

        [Command]
        public void CmdInit()
        {
            timer = 0;
            isTimerOn = false;

            cycleController = GetComponent<CycleController>();
        }

        [Command]
        public void CmdSetTimer(int sec)
        {
            timer = (float)sec;
            isTimerOn = true;

            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                go.GetComponent<Player.PlayerController>().RpcOnSetTimer(sec);
            }
        }

        [Command]
        private void CmdUpdateTimer()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = (float)0;
                isTimerOn = false;
                cycleController.CmdNextEvent();
            }
        }
    }
}