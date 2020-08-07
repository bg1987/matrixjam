using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class UIManager : MonoBehaviour
    {
        public static Dictionary<MessageLocation, MessageScript> _messages = new Dictionary<MessageLocation, MessageScript>();
        private static UIManager _instance;
        
        public GameObject DarkScreen;
        public GameObject Tooltips;
        private static MessageScript _currentMessage;

        private void Awake()
        {
            _instance = this;
            DarkScreen.SetActive(false);
            Tooltips.SetActive(true);
        }


        public static void Register(MessageLocation location, MessageScript messageScript)
        {
            _messages.Add(location, messageScript);
        }

        public static void ShowMessage(string message, MessageLocation location)
        {
            
            HideMessage();
            _currentMessage = _messages[location];
            _currentMessage.ShowMessage(message);
        }

        public static void HideMessage()
        {
            if (_currentMessage != null)
            {
                _currentMessage.HideMessage();
            }
        }

        public static void ShowDarkScreen()
        {
            _instance.DarkScreen.SetActive(true);
        }
        
        public static void HideDarkScreen()
        {
            _instance.DarkScreen.SetActive(false);
        }

        
    }
}