using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [ExecuteInEditMode]
    public class TextMeshContainer : MonoBehaviour
    {
        [SerializeField] TextMeshSizer textSizer;
        [SerializeField] Vector3 padding;
        // Start is called before the first frame update
        void Start()
        {
            //text.SetVerticesDirty//;
        }

        // Update is called once per frame
        void Update()
        {
            //Activate();
        }
        public void UpdateSize()
        {
            Vector3 scale = textSizer.lastSize * textSizer.transform.localScale;
            scale.z = transform.localScale.z;
            transform.localScale = scale + padding;
        }
        private void OnGUI()
        {
            if (Application.isPlaying)
                return;
            UpdateSize();
        }
    }
}
