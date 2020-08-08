using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixJam.Team;


namespace MatrixJam.Team4
{
    public class IntroManager
    {

        private List<MessageObject> _introMessages;
        private int _msgIndex;

        public IntroManager(List<MessageObject> introMessages)
        {
            _introMessages = introMessages;
            _msgIndex = 0;
        }

        public void nextMessage()
        {
            ++_msgIndex;
            if(_msgIndex < _introMessages.Count)
            {
                UIManager.ShowIntroMessage(_introMessages[_msgIndex]);
                return;
            } 

            // TODO fire event
        }

        public void previousMessage()
        {
            if(0 < _msgIndex)
            {
                --_msgIndex;
                UIManager.ShowIntroMessage(_introMessages[_msgIndex]);
            }
        }

    }
}
