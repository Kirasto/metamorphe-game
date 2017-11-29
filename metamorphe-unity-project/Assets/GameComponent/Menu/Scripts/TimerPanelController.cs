using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    namespace Timer
    {
        public class TimerPanelController : MonoBehaviour
        {
            float timer;
            bool isTimerOn;

            public TMPro.TMP_Text timerText;

            private void Start()
            {
                isTimerOn = false;
            }

            public void setTimer(int sec)
            {
                isTimerOn = true;
                timer = (float)sec;
            }

            // Update is called once per frame
            void Update()
            {
                if (isTimerOn)
                {
                    timer -= Time.deltaTime;
                    string text = "";
                    int min = (int)timer / 60;
                    int sec = (int)timer % 60;
                    if (min > 0)
                    {
                        text += min.ToString() + ":";
                        if (sec < 10)
                        {
                            text += "0";
                        }
                    }
                    text += sec.ToString();
                    timerText.text = text;

                    if (timer < 0)
                    {
                        timer = (float)0;
                        isTimerOn = false;
                    }
                }
            }
        }
    }
}