using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class EventManager
    {
        public static EventManager Singleton;
        public event EventIntroDone IntroDone;
        public delegate void EventIntroDone();

        public event EventBoardCreated BoardCreated;
        public delegate void EventBoardCreated();

        public event EventPlayerPlayed PlayerPlayed;
        public delegate void EventPlayerPlayed(TurnObject turnObject);

        public event EventTurnOver TurnOver;
        public delegate void EventTurnOver();       

        public void OnTurnOver()
        {
            TurnOver?.Invoke();
        }

        public void OnBoardCreated()
        {
            BoardCreated?.Invoke();
        }

        public void OnIntroDone()
        {
            IntroDone?.Invoke();
        }

        public void OnPlayerPlayed(TurnObject turnObject)
        {
            PlayerPlayed?.Invoke(turnObject);
        }

        public void Awake()
        {
            Singleton = this;
        }
    }
}
