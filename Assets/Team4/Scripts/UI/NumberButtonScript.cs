using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team4
{
    [RequireComponent(typeof(Button))]
    public class NumberButtonScript : MonoBehaviour
    {
        public Text OriginalValueTextBox;
        public Text CurrentValueTextBox;
        private int _originalValue;
        private int _currentValue;
        private Button _button;
        public Vector2 Index { get; set; }

        private void Awake()
        {
            _button = GetComponent<Button>();
            
        }

        public void Clear()
        {
            OriginalValueTextBox.text = "";
            CurrentValueTextBox.text = "";
        }

        public void SetValue(int original, int current, Color playerColor)
        {
            CurrentValueTextBox.color = playerColor;
            OriginalValueTextBox.color = playerColor;
            SetValue(original, current);
        }

        public void SetValue(int original, int current) {
            _originalValue = original;
            _currentValue = current;
            if (current == 0)
            {
                Clear();
                return;
            }
            CurrentValueTextBox.text = "" + current;
            
            if (current != original)
            {
                OriginalValueTextBox.text = "" + original;
            }
            else
            {
                OriginalValueTextBox.text = "";
            }
        }

        public void Choose()
        {
            if (_currentValue > 0)
            {
                UIManager.NumberChosen(_currentValue);
            }
            else
            {
                UIManager.SquareChosen(Index);
            }
        }

        public void MakeSelectable(bool b)
        {
            _button.interactable = b;
        }

    }
}
