using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team
{
    public class EventManager
    {
        public static EventManager Singleton;
        public event EventBoardCreated BoardCreated;
        public delegate void EventBoardCreated();

        public event EventIntroDone IntroDone;
        public delegate void EventIntroDone();

        public void OnBoardCreated()
        {
            BoardCreated?.Invoke();
        }

        public void OnIntroDone()
        {
            IntroDone?.Invoke();
        }

        public void Awake()
        {
            Singleton = this;
        }
    }
}
