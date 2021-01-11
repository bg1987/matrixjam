using System.Collections;
using System.Collections.Generic;
using MatrixJam;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam
{
    [UnitySingleton(UnitySingletonAttribute.Type.ExistsInScene, false)]
    public class GlobalUI : UnitySingleton<GlobalUI>
    {

        private const int TOTAL_SKIPS = 7;
        private int _currentSkipsAmount = 0;
        
        [Header("UI")] 
        public GameObject Container;
        public Text SkipsLeftText;
        public Text GamesDoneText;
        public Button UndoButton;
        public Button SkipButton;
        public Button RestartButton;
        public Button ExitButton;
        public Button XButton;
        
        // Start is called before the first frame update
        void Start()
        {
            // initialize buttons
            XButton.onClick.AddListener(() =>
            {
                ToggleUI();
            });
            
            ExitButton.onClick.AddListener(() =>
            {
                MatrixTraveler.Instance.LoadStartScene();
                ToggleUI();
            });

            RestartButton.onClick.AddListener(() =>
            {
                MatrixTraveler.Instance.ReTravelToCurrentGame();
                ToggleUI();
            });
            
            SkipButton.onClick.AddListener(() =>
            {
               SkipLevel();
               ToggleUI();
            });
            
            UndoButton.onClick.AddListener(() =>
            {
                UndoLevel();
                ToggleUI();
            });
            
            // initialize text feilds
            UpdateSkipsFields(_currentSkipsAmount.ToString(), TOTAL_SKIPS.ToString());
            // TODO: link total number of levels
            UpdateGamesDoneFields("0", "7");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleUI();
            }
        }

        public void ToggleUI()
        {
            // enable object
            Container.SetActive(!Container.activeSelf);
            
            // TODO: pause game
            //Time.timeScale = Container.activeSelf ? 0 : 1;
            //LevelHolder.Pause(Container.activeSelf)
        }

        public void UpdateSkipsFields(string skipsLeft, string skipsTotal)
        {
            SkipsLeftText.text = $"Skip Level {skipsLeft}/{skipsTotal}";
        }
        
        public void UpdateGamesDoneFields(string gamesDone, string gamesDoneTotal)
        {
            GamesDoneText.text = $"{gamesDone}/{gamesDoneTotal}";
        }

        public void SkipLevel()
        {
            if (_currentSkipsAmount >= TOTAL_SKIPS)
                return;
            
            // TODO: get the next level scene name
            // SceneManager.SceneMang.LoadScene(scene level name);
            
            _currentSkipsAmount++;

            UpdateSkipsFields(_currentSkipsAmount.ToString(), TOTAL_SKIPS.ToString());
        }
        
        private void UndoLevel()
        {
            throw new System.NotImplementedException();
        }
    }
}
