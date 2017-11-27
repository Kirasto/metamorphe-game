using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    namespace Annoucement
    {
        public class AnnouncementPanelController : MonoBehaviour
        {
            public GameObject titleAnnoucementPanel;
            public TMPro.TMP_Text titleAnnoucement;

            private float timer;
            private bool isOnTitleAnnoucement;

            public void Start()
            {
                timer = 0;
                isOnTitleAnnoucement = false;
            }

            public void Update()
            {
                if (isOnTitleAnnoucement)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        timer = 0;
                        isOnTitleAnnoucement = false;
                        setActiveTitleAnnoucementPanel(false);
                    }
                }
            }

            private void setActiveTitleAnnoucementPanel(bool isActive)
            {
                titleAnnoucementPanel.SetActive(isActive);
            }

            public void setTitleAnnoucement(string titleAnnoucementText, int time = 5)
            {
                setActiveTitleAnnoucementPanel(true);
                titleAnnoucement.text = titleAnnoucementText;
                setTimer(time);
            }

            private void setTimer(int time = 5)
            {
                timer = time;
                isOnTitleAnnoucement = true;
            }
        }
    }
}