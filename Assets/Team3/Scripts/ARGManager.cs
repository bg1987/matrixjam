using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MatrixJam.Team3
{
    public class ARGManager : MonoBehaviour
    {
        [SerializeField] private GameObject currentlEnabled;
        [SerializeField] private string currentlEnabledString;
        [SerializeField] private Dictionary<string, GameObject> Stages;

        [SerializeField] private InputField _inputField; 
        
        [SerializeField] private UnityEvent [] exitEvent;

        
        [Serializable]
        public struct StageItem {
            public string name;
            public GameObject theObject;
            public string description;
        }
        
        [SerializeField] private List<StageItem> StagesList;
        
        [SerializeField] public List<GameObject> InitialObjects;
        
        // Start is called before the first frame update
        void Start()
        {
            if (Stages == null)
            {
                CreateStages();
            }
            
           
        }

        private void CreateStages()
        {
            Stages = new Dictionary<string, GameObject>();
            foreach ( StageItem item in StagesList)
            {
                
                Stages[item.name.ToLower()] = item.theObject;
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
                currentlEnabled.SetActive(true);
                _inputField.text = "";
            }
        }


        public void handleExit(int exit_num)
        {
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
