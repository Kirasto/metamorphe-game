using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    namespace Effect
    {
        public class EffectPanelContoller : MonoBehaviour
        {

            public GameObject blindEffectPanel;

            public void setBlindEffect(bool isActive)
            {
                blindEffectPanel.SetActive(isActive);
            }
        }
    }
}