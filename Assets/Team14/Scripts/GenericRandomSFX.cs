using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team
{
    [Serializable]
    public class GenericRandomSFX 
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip[] clips;
        private AudioClip GetRandom()
        {
            if(clips.Length == 0)
            {
                return null;
            }

            var i = UnityEngine.Random.Range(0, clips.Length);
            return clips[i];
        }

        public void PlayRandom()
        {
            var Randomclip = GetRandom();
            if (!Randomclip) return;
            source.clip = Randomclip;
            source.Stop();
            source.Play();
        }
    }
}
