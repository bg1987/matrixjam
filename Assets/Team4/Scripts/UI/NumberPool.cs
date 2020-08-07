using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class NumberPool : MonoBehaviour
    {
        public bool HumanControlled;
        public NumberButtonScript NumberButtonPrefab;

        private NumberButtonScript[] _numbers; 

        private Color _playerColor;

        // Start is called before the first frame update
        void Awake()
        {
            _playerColor = NumberButtonPrefab.CurrentValueTextBox.color;
            _numbers= new NumberButtonScript[NumberButtonsManager.BOARD_SIZE];
            _numbers[0] = NumberButtonPrefab;
            NumberButtonPrefab.SetValue(1, 1, _playerColor);
            var squareWidth = NumberButtonsManager.SquareWidth(NumberButtonPrefab);
            var originalPosition = NumberButtonPrefab.transform.localPosition;
            for (int i = 1; i < NumberButtonsManager.BOARD_SIZE; i++)
            {
                var x = i % 2;
                var y = i / 2;
                var clone = Instantiate(NumberButtonPrefab, NumberButtonPrefab.transform.parent);
                clone.SetValue(i+1, i+1, _playerColor);
                clone.transform.localPosition = originalPosition + new Vector3(x * squareWidth, -y * squareWidth);
                _numbers[i] = clone;
            }
            
        }

        public void SetAvailableNumbers(List<int> choices)
        {
            for (int i = 0; i < _numbers.Length; i++)
            {
                if (choices.Count > i)
                {
                    _numbers[i].gameObject.SetActive(true);
                    _numbers[i].SetValue(choices[i], choices[i]);
                    _numbers[i].MakeSelectable(HumanControlled);
                }
                else
                {
                    _numbers[i].gameObject.SetActive(false);
                }
            }
        }
        
        public void MakeNoneSelectable()
        {
            for (int i = 0; i < _numbers.Length; i++)
            {
                
                _numbers[i].MakeSelectable(false);
                
            }
        }
    }
}
