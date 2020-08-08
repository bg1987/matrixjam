using System;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class IntroManager : MonoBehaviour
    {

        public MessageScript[] Messages;
        private int _msgIndex;

        private void Start()
        {
            Messages[_msgIndex].ShowMessage();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextMessage();
            }
        }

        public void NextMessage()
        {
            if (Messages[_msgIndex].HasNext())
            {
                Messages[_msgIndex].ShowNext();
            }
            else
            {
                MoveToTheNextTooltip();
            }

            
        }

        private void MoveToTheNextTooltip()
        {
            Messages[_msgIndex].HideMessage();
            if (_msgIndex < Messages.Length - 1)
            {
                _msgIndex++;
                Messages[_msgIndex].ShowMessage();
            }
        }

        public void PreviousMessage()
        {
            if(0 < _msgIndex)
            {
                --_msgIndex;
                Messages[_msgIndex].ShowMessage();
            }
        }

    }
}
