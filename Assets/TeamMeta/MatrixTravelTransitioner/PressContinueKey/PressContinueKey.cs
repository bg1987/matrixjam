using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixTravelTransition
{
    public class PressContinueKey : MonoBehaviour
    {
        [SerializeField] KeyCode continueKey = KeyCode.Space;

        bool wasContinueKeyPressed = false;
        bool isActive = false;

        [SerializeField] PressContinueKeyUI pressContinueKey;

        // Start is called before the first frame update
        void Start()
        {
            Reset();
        }
        // Update is called once per frame
        void Update()
        {
            if (!isActive)
                return;
            if (Input.anyKeyDown)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    //Debug.Log("mouse was pressed");
                    return;
                }

                var isContinueKeyPressedNow = Input.GetKey(continueKey);
                if (isContinueKeyPressedNow)
                {
                    wasContinueKeyPressed = true;
                }
                else
                {
                    pressContinueKey.Appear();
                }
            }
            else
            {
                if(pressContinueKey.IsVisible())
                    pressContinueKey.Disappear();
            }

        }
        void Reset()
        {
            wasContinueKeyPressed = false;
            pressContinueKey.Reset();
            pressContinueKey.SetKey(continueKey);
        }
        public void Activate()
        {
            Reset();
            isActive = true;
        }
        public void Deactivate()
        {
            pressContinueKey.Disappear();
            isActive = false;
        }
        public bool WasContinueKeyPressed()
        {
            return wasContinueKeyPressed;
        }
        private void OnDisable()
        {
            pressContinueKey.DisappearInstantly();
        }
    }
}
