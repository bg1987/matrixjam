using System;
using UnityEngine;

namespace MatrixJam.Team14
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
        public void PlayRandomPitch(float pitchRandom = 1)
        {
            var Randomclip = GetRandom();
            if (!Randomclip) return;
            source.clip = Randomclip;
            source.Stop();
            source.Play();
            source.pitch = UnityEngine.Random.Range(0.7f, 1.5f);
        }

    }
}
