using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace MatrixJam.TeamMeta
{
    public class MatrixNode : Node
    {
        public int index;
        public string levelName;
        public string scenePath;
        public ColorHdr colorHdr1 = new ColorHdr() {color = new Color(0.184f, 0.184f, 0.749f, 1),intensity = 2.4f };
        public ColorHdr colorHdr2 = new ColorHdr() {color = new Color(0.11f, 0.75f, 0.6f, 1),intensity = 2.9f };

        public void GenerateFields()
        {
            GenerateLevelNameLabel();
            GenerateLevelNameInputField();

            GenerateScenePathLabel();
            GenerateScenePathInputField();

            GenerateColorsLabel();
            GenerateColorsFields();
            
        }
        void GenerateLevelNameLabel()
        {
            var label = new Label("Level Name");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.backgroundColor = Color.black;
            this.mainContainer.Add(label);
        }
        void GenerateLevelNameInputField()
        {
            var textField = new TextField("");
            textField.RegisterValueChangedCallback(evt =>
            {
                this.levelName = evt.newValue;
            });
            textField.SetValueWithoutNotify(this.levelName);

            var childrenEnumerator = textField.Children().GetEnumerator();
            childrenEnumerator.MoveNext();
            var inputField = childrenEnumerator.Current;
            inputField.style.flexBasis = 1;

            this.mainContainer.Add(textField);
        }
        void  GenerateScenePathLabel()
        {
            var label = new Label("Scene Path");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.backgroundColor = Color.black;
            this.mainContainer.Add(label);
        }
        void GenerateScenePathInputField()
        {
            var textField = new TextField("");
            textField.RegisterValueChangedCallback(evt =>
            {
                this.scenePath = evt.newValue;
            });
            textField.SetValueWithoutNotify(this.scenePath);

            var childrenEnumerator =  textField.Children().GetEnumerator();
            childrenEnumerator.MoveNext();
            var inputField = childrenEnumerator.Current;
            inputField.style.flexBasis = 1;

            this.mainContainer.Add(textField);
        }
        void GenerateColorsLabel()
        {
            var label = new Label("Colors");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.backgroundColor = Color.black;
            this.mainContainer.Add(label);
        }
        void GenerateColorsFields()
        {
            GenerateColorOneField();
            GenerateColorTwoField();
        }
        void GenerateColorOneField()
        {
            VisualElement colorHdrField = GenerateColorHdrField();

            var colorField = colorHdrField.Query<ColorField>().First();
            var intensityField = colorHdrField.Query<FloatField>().First();
            
            colorField.SetValueWithoutNotify(this.colorHdr1.color);
            colorField.RegisterValueChangedCallback(evt => this.colorHdr1.color = evt.newValue);

            intensityField.SetValueWithoutNotify(this.colorHdr1.intensity);
            intensityField.RegisterValueChangedCallback(evt => this.colorHdr1.intensity = evt.newValue);
        }
        void GenerateColorTwoField()
        {
            VisualElement colorHdrField = GenerateColorHdrField();

            var colorField = colorHdrField.Query<ColorField>().First();
            var intensityField = colorHdrField.Query<FloatField>().First();

            colorField.SetValueWithoutNotify(this.colorHdr2.color);
            colorField.RegisterValueChangedCallback(evt => this.colorHdr2.color = evt.newValue);

            intensityField.SetValueWithoutNotify(this.colorHdr2.intensity);
            intensityField.RegisterValueChangedCallback(evt => this.colorHdr2.intensity = evt.newValue);
        }
        VisualElement GenerateColorHdrField()
        {
            VisualElement colorHdrField = new VisualElement();

            ColorField colorField = GenerateColorField();
            FloatField intensityField = GenerateColorIntensityField();

            colorHdrField.Add(colorField);
            colorHdrField.Add(intensityField);

            colorHdrField.style.flexDirection = FlexDirection.Row;

            this.mainContainer.Add(colorHdrField);

            return colorHdrField;
        }
        ColorField GenerateColorField()
        {
            ColorField colorField = new ColorField();

            colorField.style.flexBasis = 0;
            colorField.style.flexGrow = 1;

            return colorField;
        }
        FloatField GenerateColorIntensityField()
        {
            FloatField floatField = new FloatField();

            return floatField;
        }
    }
}
