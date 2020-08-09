using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MatrixJam.Team3
{
    public class ARGManager : MonoBehaviour
    {
        [SerializeField] private bool NextCharacter;
        [SerializeField] private bool YoutubeEnabled;
        [SerializeField] private bool TextEnabled;
        [SerializeField] private TextMeshProUGUI textToDisplay; 
        
        [SerializeField] private GameObject currentlEnabled;
        [SerializeField] private string currentlEnabledString;
        [SerializeField] private Dictionary<string, GameObject> Stages;
        [SerializeField] private Dictionary<string, string> Descriptions;
        [SerializeField] private Dictionary<string, bool> forceTexts;

        [SerializeField] private InputField _inputField; 
        
        [SerializeField] private UnityEvent [] exitEvent;

        
        [Serializable]
        public struct StageItem {
            public string name;
            public GameObject theObject;
            public string description;
            public bool forceText;
        }
        
        [SerializeField] private List<StageItem> StagesList;
        
        [SerializeField] public List<GameObject> InitialObjects;
        
        [SerializeField] public List<int> levels;
        [SerializeField] private int currentLevel;
        [SerializeField] private int numLevels =0;
        [SerializeField] private int MaxLevels =5;
        
        // Start is called before the first frame update
        void Start()
        {
            if (Stages == null)
            {
                CreateStages();
            }            
           
        }

        public void StartLevel(int level)
        {
            if (levels == null)
            {
                levels = new List<int>();
                for (int i = 0; i < MaxLevels; i++)
                {
                    levels.Add((level + i) % MaxLevels);
                }
            }

            currentLevel = level;
            ChangeItem("000" + currentLevel);
        }
        
        private void CreateStages()
        {
            Stages = new Dictionary<string, GameObject>();
            Descriptions = new Dictionary<string, string>();
            forceTexts = new Dictionary<string, bool>();
            foreach ( StageItem item in StagesList)
            {
                
                Stages[item.name.ToLower()] = item.theObject;
                Descriptions[item.name.ToLower()] = item.description;
                forceTexts[item.name.ToLower()] = item.forceText;
            }
            
            
        }


        public void ChangeItem(string item)
        {
            /*foreach (GameObject go in InitialObjects)
            {
                go.SetActive(false);
            }*/

            if (Stages == null)
            {
                CreateStages();
            }
            if (Stages.ContainsKey(item.ToLower())){
                currentlEnabled.SetActive(false);
                currentlEnabled = Stages[item.ToLower()];
                currentlEnabledString = item.ToLower();
                _inputField.text = "";

                if (TextEnabled || forceTexts[item.ToLower()])
                {
                    textToDisplay.text = Descriptions[item.ToLower()];
                }
                if (!TextEnabled)
                {
                    currentlEnabled.SetActive(true);
                }
                
            }
        }


        public void handleExit(int exit_num)
        {
            if (NextCharacter)
            {
                if (numLevels++ < MaxLevels -1)
                {
                    currentLevel = (currentLevel + 1 ) % MaxLevels;
                    ChangeItem("000" + currentLevel);
                    return;
                }

                Debug.Log("numlevels =" + numLevels + "maxlelevs   = " + (MaxLevels ) );
                if (YoutubeEnabled && numLevels == MaxLevels )
                {
                    return;
                }
            }
            
            
            // do not move to the next character
            exitEvent[exit_num].Invoke();
            
        }

        public void OpenWebsite(string URL)
        {
            Application.OpenURL(URL);
        }
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
