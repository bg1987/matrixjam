using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [ExecuteInEditMode]
    public class TextMeshSizer : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        
        RectTransform textRT;
        public Vector2 lastSize { get; private set; }
        string lastText;
        // Start is called before the first frame update
        void Awake()
        {
            textRT = GetComponent<RectTransform>();

        }
        private void Start()
        {
            UpdateTextSize();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        private void OnGUI()
        {
            if (Application.isPlaying)
                return;
            UpdateTextSize();
        }
        public void UpdateTextSize()
        {
            if (text.text == lastText)
                return;
            Vector2 preferredSize = text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);

            textRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize.x);
            textRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);

            lastSize = textRT.rect.size;
            lastText = text.text;
        }
    }
}
