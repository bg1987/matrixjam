using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team14
{
    [Serializable]
    public class AnimateThomasAfter
    {
        public bool resetEveryRestart;
        public float secsDelay;

        private float _currSecs;

        public void OnUpdate()
        {
            _currSecs += Time.deltaTime;
        }


        public void OnRestart()
        {
            if (resetEveryRestart) _currSecs = 0f;
        }

        public bool ShouldAnimate => _currSecs >= secsDelay;
    }
    
    public class ThomasMoon : MonoBehaviour
    {
        public enum LookAt
        {
            Cursor,
            NextObstacleOrTarget,
            Target
        }

        [Range(0.1f, 1f)]
        [SerializeField] private float lookLerpSpeed = 0.6f;
        [SerializeField] private AnimateThomasAfter animationDelay; // Hold off animations so player isnt distracted
        [SerializeField] private Vector2 eyebrowsRandomDelta;
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
        private const string TrigReset = "reset";
        private float _timeSinceRestart = 0f;

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
                    case LookAt.NextObstacleOrTarget:
                        return GetPOsNextObstacleOrTarget(); 
                    case LookAt.Target:
                        return Target.position;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Vector3 GetPOsNextObstacleOrTarget()
        {
            var pos = transform.position;
            var nextObstacle = Obstacle.GetNextObstacle(pos);
            if (!nextObstacle) return target.position;
            
            var nextObstaclePos = nextObstacle.transform.position;
            var targetPos = target.position;

            var isTargetCloser = Vector3.Distance(pos, targetPos) 
                                 < Vector3.Distance(pos, nextObstaclePos);

            return isTargetCloser ? targetPos : nextObstaclePos;
        }

        private Vector3 startScale;

        private IEnumerator Start()
        {
            startScale = transform.localScale;
            StartCoroutine(RandomEyebrows(eyebrowsRandomDelta[0], eyebrowsRandomDelta[1]));
            yield return new WaitForSeconds(1);
            GameManager.ResetEvent += OnRestart;
            // HappyAnim(3f);
        }

        private void OnDestroy()
        {
            GameManager.ResetEvent -= OnRestart;
        }

        private void Update()
        {
            animationDelay.OnUpdate();
            transform.localScale += Vector3.one * Time.deltaTime * scalePerSec;
            HandleEyesLookAt();
        }

        public void WeirdAnim(float time)
        {
            if (!animationDelay.ShouldAnimate) return;

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
            if (!animationDelay.ShouldAnimate) return;

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
            if (!animationDelay.ShouldAnimate) return;

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

        private void OnRestart()
        {
            animationDelay.OnRestart();
            StopAllCoroutines();
            anim.SetTrigger(TrigReset);
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

            var oldPos = eyeSprite.transform.localPosition;
            var targetPos = delta.normalized * localEyeRadius;

            eyeSprite.transform.localPosition = Vector3.Lerp(oldPos, targetPos, lookLerpSpeed);
        }
    }
}
