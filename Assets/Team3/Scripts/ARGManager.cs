using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team3
{
    public class ARGManager : MonoBehaviour
    {
        [SerializeField] private GameObject currentlEnabled;
        [SerializeField] private Dictionary<string, GameObject> Stages;        
        
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
            foreach (GameObject go in InitialObjects)
            {
                go.SetActive(false);
            }

            if (Stages == null)
            {
                CreateStages();
            }
            if (Stages.ContainsKey(item.ToLower())){
                currentlEnabled.SetActive(false);
                currentlEnabled = Stages[item.ToLower()];
                currentlEnabled.SetActive(true);
            }
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
