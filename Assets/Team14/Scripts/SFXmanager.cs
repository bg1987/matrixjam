using MatrixJam.Team14;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team
{
    public class SFXmanager : MonoBehaviour
    {
        [Header("Sound Effects")]
        [SerializeField] private AudioSource Jump;
        [SerializeField] private AudioSource Railway;
        [SerializeField] private AudioSource Honk;
        [SerializeField] private AudioSource AwakeBells;
        [SerializeField] private AudioSource Duck;
        
        public void PlaySFX(TrainMove move)
        {
            switch (move)
            {
                case TrainMove.Jump:
                    Jump.Play();
                    break;
                case TrainMove.Duck:
                    
                    break;
                case TrainMove.Honk:
                    break;
            }
        }
    }
}
