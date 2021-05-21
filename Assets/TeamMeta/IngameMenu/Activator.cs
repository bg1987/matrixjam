using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Activator : MonoBehaviour
    {
        [SerializeField] IngameMenuBG IngameMenuBG;
        [SerializeField] List<Selection> selections;
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
        public void Deactivate()
        {
            IngameMenuBG.Disappear(bgDisappearDuration);
            IngameMenuBG.SetInteractable(false);

            foreach (var selection in selections)
            {
                selection.Disappear(bgDisappearDuration, DeactivateSelection);
                selection.SetInteractable(false);
            }

            isActivated = false;
        }
        void DeactivateImmediately()
        {
            IngameMenuBG.Disappear(0);
            IngameMenuBG.SetInteractable(false);

            foreach (var selection in selections)
            {
                selection.Disappear(0, DeactivateSelection);
                selection.SetInteractable(false);
            }

            isActivated = false;
        }
        void Activate()
        {
            IngameMenuBG.Appear(bgAppearDuration);
            IngameMenuBG.SetInteractable(true);

            foreach (var selection in selections)
            {
                selection.gameObject.SetActive(true);
                selection.Appear(bgAppearDuration);
                selection.SetInteractable(true);
            }
            
            isActivated = true;
        }
        void DeactivateSelection(Selection selection)
        {
            selection.gameObject.SetActive(false);
        }
    }
}
