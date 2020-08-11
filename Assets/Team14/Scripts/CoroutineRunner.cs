using System.Collections;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        private static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    new GameObject("CoroutineRunner", typeof(CoroutineRunner));
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Whoops! shouldnt have two of these");
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance != this) return;
            _instance = null;
        }

        public static Coroutine StartCoroutineStatic(IEnumerator routine) => Instance.StartCoroutine(routine);
        public static void StopCoroutineStatic(Coroutine routine) => Instance.StopCoroutine(routine);
    }
}
