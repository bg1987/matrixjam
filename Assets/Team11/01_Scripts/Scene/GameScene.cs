using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] protected float _enterDelayTime = 0.5f;
        [SerializeField] protected float _blackOutTime = 1f;
        [SerializeField] protected float _exitDelayTime = 0.5f;
        [SerializeField] protected float _blackInTime = 1f;
        [SerializeField] protected Color _exitColor = Color.black;

        public float EnterDelayTime { get { return this._enterDelayTime; } }
        public float BlackOutTime { get { return this._blackOutTime; } }
        public float ExitDelayTime { get { return this._exitDelayTime; } }
        public float BlackInTime { get { return this._blackInTime; } }
        public Color ExitColor { get { return this._exitColor; } }

        public virtual void Enter()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Exit()
        {
            this.gameObject.SetActive(false);
        }
    }
}
