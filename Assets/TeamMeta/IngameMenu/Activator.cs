using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Activator : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] IngameMenuOverlay IngameMenuBG;
        [SerializeField] MenuActivator ingameMenuActivator;
        [SerializeField] bool isListeningToActivationKey = false;
        bool isActivated = false;

        [Header("Appearance")]
        [SerializeField] float bgAppearDuration = 0.5f;
        [SerializeField] float bgDisappearDuration = 0.5f;
        void Awake()
        {
            container.SetActive(true);
        }
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
        public void Deactivate()
        {
            if (!isActivated)
                return;
            IngameMenuBG.Disappear(bgDisappearDuration);
            IngameMenuBG.SetInteractable(false);
            IngameMenuBG.Deactivate(bgDisappearDuration);

            ingameMenuActivator.Deactivate();

            isActivated = false;
        }
        public void DeactivateImmediately()
        {
            IngameMenuBG.Disappear(0);
            IngameMenuBG.SetInteractable(false);
            IngameMenuBG.Deactivate(0);

            ingameMenuActivator.DeactivateImmediately();

            isActivated = false;
        }
        void Activate()
        {
            if (isActivated)
                return;
            IngameMenuBG.Activate();
            IngameMenuBG.Appear(bgAppearDuration);
            IngameMenuBG.SetInteractable(true);

            ingameMenuActivator.Activate();

            isActivated = true;
        }
    }
}
