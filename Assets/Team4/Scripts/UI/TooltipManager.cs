using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class TooltipManager
    {
        public static Dictionary<MessageLocation, MessageScript> _messages = new Dictionary<MessageLocation, MessageScript>();
        private static MessageScript _currentMessage;

        public static void RegisterMessageBox(MessageLocation location, MessageScript messageScript)
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
    }
}