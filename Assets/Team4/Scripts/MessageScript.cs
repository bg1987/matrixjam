using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team4
{
    public class MessageScript : MonoBehaviour
    {
        public MessageLocation Location;
        public GameObject HighligtedObject;
        public Text Textbox;

        private Transform _originalParent;

        public void Awake()
        {
            UIManager.Register(Location, this);
            gameObject.SetActive(false);
        }

        public void ShowMessage(string message)
        {
            if (HighligtedObject != null)
            {
                UIManager.ShowDarkScreen();
                _originalParent = HighligtedObject.transform.parent;
                HighligtedObject.transform.SetParent(transform.parent);
                transform.SetParent(HighligtedObject.transform);

            }
            
            Textbox.text = message;
            gameObject.SetActive(true);
            

        }
        
        public void HideMessage() {
            if (HighligtedObject != null)
            {
                transform.SetParent(HighligtedObject.transform.parent);
                HighligtedObject.transform.SetParent(_originalParent);
                UIManager.HideDarkScreen();
            }
            gameObject.SetActive(false);
    
        }
        
        
        
    }
    
    

    public enum MessageLocation
    {
        AboveBoard,
        NextToScore,
        PlayerPool,
        PlayerWaitQueue,
        EnemyPool,
        AttackOptions


    }
}
