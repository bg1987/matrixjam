using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team14
{
    public class ThomasMoon : MonoBehaviour
    {
        public enum LookAt
        {
            Cursor,
            Target
        }
        
        [SerializeField] private Camera cam;
        [SerializeField] private float localEyeRadius;
        [SerializeField] private float scalePerSec;

        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer faceSprite;
        [SerializeField] private SpriteRenderer leftEye;
        [SerializeField] private SpriteRenderer rightEye;
        [SerializeField] private SpriteRenderer leftEyebrow;
        [SerializeField] private SpriteRenderer rightEyebrow;
        [SerializeField] private LookAt lookMode;
        [SerializeField] private Transform target;

        public float Z => faceSprite.transform.position.z;

        private bool _ignoreEyes;
        private const string TrigWeird = "weird";
        private const string TrigEyebrows = "eyebrows";
        private const string TrigHappy = "happy";

        public LookAt LookMode
        {
            get => lookMode;
            set => lookMode = value;
        }

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        private Vector3? LookAtPos
        {
            get
            {
                if (_ignoreEyes) return null;
                
                switch (LookMode)
                {
                    case LookAt.Cursor:
                        var screenPos = Input.mousePosition;
                        screenPos.z = Z;
                        return cam.ScreenToWorldPoint(screenPos);
                    case LookAt.Target:
                        return Target.position;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Vector3 startScale;

        private IEnumerator Start()
        {
            startScale = transform.localScale;
            StartCoroutine(RandomEyebrows(3f, 3f));
            yield return new WaitForSeconds(1);
            // HappyAnim(3f);
        }

        private void Update()
        {
            transform.localScale += Vector3.one * Time.deltaTime * scalePerSec;
            HandleEyesLookAt();
        }

        private IEnumerator RandomEyebrows(float minTime, float maxTime, bool startImmediate = false)
        {
            if (startImmediate) EyebrowsAnim();
            while (true)
            {
                var randTime = Random.Range(minTime, maxTime); 
                yield return new WaitForSeconds(randTime);
                EyebrowsAnim();
            }
        }

        public void WeirdAnim(float time)
        {
            StartCoroutine(WierdRoutine());
            IEnumerator WierdRoutine()
            {
                StartAnim(TrigWeird, false);
                yield return new WaitForSeconds(time);
                EndAnim(TrigWeird);
            }
        }
        
        public void HappyAnim(float time)
        {
            StartCoroutine(HappyRoutine());
            IEnumerator HappyRoutine()
            {
                StartAnim(TrigHappy, false);
                yield return new WaitForSeconds(time);
                EndAnim(TrigHappy);
            }
        }

        public void EyebrowsAnim()
        {
            const float dur1 = 0.4f;
            const float delay = 1f;
            const float dur2 = 0.6f;

            var oldLeft = leftEye.transform.localPosition;
            var oldRight = rightEye.transform.localPosition;
            var leftTween1 = leftEye.transform.DOLocalMove(Vector3.zero, dur1);
            var rightTween1 = rightEye.transform.DOLocalMove(Vector3.zero, dur1);
            
            var leftTween2 = leftEye.transform.DOLocalMove(oldLeft, dur2);
            var rightTween2 = rightEye.transform.DOLocalMove(oldRight, dur2);
            

            var seq = DOTween.Sequence()
                .Append(leftTween1)
                .Join(rightTween1)
                .AppendInterval(delay)
                .Append(leftTween2)
                .Join(rightTween2);
            
            seq.onComplete = () => EndAnim(TrigEyebrows);

            Debug.Log("EYEBROWS!");
            StartAnim(TrigEyebrows, true);
            seq.Play();
        }

        private void StartAnim(string trig, bool ignoreEyes)
        {
            _ignoreEyes = ignoreEyes;
            anim.SetBool(trig, true);
        }

        private void EndAnim(string trig)
        {
            _ignoreEyes = false;
            anim.SetBool(trig, false);
        }

        private bool HandleEyesLookAt()
        {
            var lookAtNullable = LookAtPos;

            if (!lookAtNullable.HasValue) return false;
            var lookAt = lookAtNullable.Value;
            
            EyeLookAt(leftEye, lookAt);
            EyeLookAt(rightEye, lookAt);
            
            return true;
        }

        public void RestartSize()
        {
            transform.localScale = startScale;
        }

        private void EyeLookAt(SpriteRenderer eyeSprite, Vector3 target)
        {
            var delta = transform.InverseTransformDirection(target - eyeSprite.transform.position);
            eyeSprite.transform.localPosition = delta.normalized * localEyeRadius;
        }
    }
}
