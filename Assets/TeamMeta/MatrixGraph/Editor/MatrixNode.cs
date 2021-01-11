using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace MatrixJam.TeamMeta
{
    public class MatrixNode : Node
    {
        public int index;
        public string levelName;
        public string scenePath;

        public void GenerateLevelNameLabel()
        {
            var label = new Label("Level Name");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.backgroundColor = Color.black;
            this.mainContainer.Add(label);
        }
        public void GenerateLevelNameInputField()
        {
            var textField = new TextField("");
            textField.RegisterValueChangedCallback(evt =>
            {
                this.levelName = evt.newValue;
            });
            textField.SetValueWithoutNotify(this.levelName);
            var scenePathStyle = textField.style;
            scenePathStyle.maxWidth = 126f;
            this.mainContainer.Add(textField);
        }
        public void  GenerateScenePathLabel()
        {
            var label = new Label("Scene Path");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.backgroundColor = Color.black;
            this.mainContainer.Add(label);
        }
        public void GenerateScenePathInputField()
        {
            var textField = new TextField("");
            textField.RegisterValueChangedCallback(evt =>
            {
                this.scenePath = evt.newValue;
            });
            textField.SetValueWithoutNotify(this.scenePath);
            var scenePathStyle = textField.style;
            scenePathStyle.maxWidth = 126f;
            this.mainContainer.Add(textField);
        }
    }
}
