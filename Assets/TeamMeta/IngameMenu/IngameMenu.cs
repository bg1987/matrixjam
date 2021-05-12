using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class IngameMenu : MonoBehaviour
    {
        [SerializeField] IngameMenuBG IngameMenuBG;
        [SerializeField] bool isListeningToActivationKey = false;
        bool isActivated = false;

        [Header("Appearance")]
        [SerializeField] float bgAppearDuration = 0.5f;
        [SerializeField] float bgDisappearDuration = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            DeactivateImmediately();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isListeningToActivationKey)
                return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isActivated)
                    Deactivate();
                else
                    Activate();
            }
        }
        public void ListenToActivationKey(bool shouldListen)
        {
            isListeningToActivationKey = shouldListen;
        }
        void Deactivate()
        {
            IngameMenuBG.Disappear(bgDisappearDuration);
            IngameMenuBG.SetInteractable(false);
            isActivated = false;
        }
        void DeactivateImmediately()
        {
            IngameMenuBG.Disappear(0);
            IngameMenuBG.SetInteractable(false);

            isActivated = false;
        }
        void Activate()
        {
            IngameMenuBG.Appear(bgAppearDuration);
            IngameMenuBG.SetInteractable(true);
            
            isActivated = true;
        }
    }
}
