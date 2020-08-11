using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeReference] protected GameScene _initialScenePrefab;
        [SerializeReference] protected BlackoutFader _fader;
        [SerializeReference] protected GameObject _temporaryPresence;

        public GameScene CurrentScene { get; protected set; }
        public GameScene CurrentScenePrefab { get; protected set; }

        private Queue<GameScene> _nextScenePrefab;
        private Coroutine _business;
        public bool IsBusy { get { return this._business != null; } }

        private void Awake()
        {
            this._nextScenePrefab = new Queue<GameScene>(2);
            this._business = null;
        }

        private void Start()
        {
            this.GoToScene(this._initialScenePrefab);
        }

        private void OnEnable()
        {
            if (Object.FindObjectsOfType<SceneManager>().Length > 1)
            {
                Debug.LogError($"{this.name}: more than one SceneManager present in the scene!!!");
            }
        }

        public void GoToScene(GameScene scenePrefab)
        {
            this._nextScenePrefab.Enqueue(scenePrefab);
            this.CurrentScenePrefab = scenePrefab;

            if (this.IsBusy == false)
            {
                this._business = this.StartCoroutine(this.ProcessScenesQueueCoroutine());
            }
        }

        public void RestartCurrentScene()
        {
            this.GoToScene(this.CurrentScenePrefab);
        }

        protected virtual IEnumerator ProcessScenesQueueCoroutine()
        {
            while (this._nextScenePrefab.Count > 0)
            {
                /* black out */
                if (this.CurrentScene != null)
                {
                    this._fader.FadeColor = this.CurrentScene.ExitColor;
                    yield return this.StartCoroutine(this._fader.FadeIn(this.CurrentScene.BlackInTime, 0f, false));
                }

                while (this._nextScenePrefab.Count > 0)
                {
                    if (this.CurrentScene != null)
                    {
                        yield return new WaitForSeconds(this.CurrentScene.ExitDelayTime);

                        this.CurrentScene.Exit();
                        this._temporaryPresence.SetActive(true);

                        SceneManager.Destroy(this.CurrentScene);
                    }

                    GameScene nextScenePrefab = this._nextScenePrefab.Peek();
                    if (nextScenePrefab != null)
                    {
                        this.CurrentScene = SceneManager.Instantiate<GameScene>(nextScenePrefab);

                        this._temporaryPresence.SetActive(false);
                        this.CurrentScene.Enter();

                        yield return new WaitForSeconds(this.CurrentScene.EnterDelayTime);
                    }
                    else
                    {
                        this.CurrentScene = null;
                    }
                    /* scene processed - dequeue it */
                    this._nextScenePrefab.Dequeue();
                }

                /* black in */
                if (this.CurrentScene != null)
                {
                    yield return this.StartCoroutine(this._fader.FadeOut(this.CurrentScene.BlackOutTime));
                }
            }

            this._business = null;
        }
    }
}
