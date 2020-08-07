using System;
using System.Collections.Generic;
using MatrixJam.Team;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class FakeChoiceManager : MonoBehaviour, IChoiceManager
    {
        public UIManager UiManagerToChange;
        private int _currentNumberValue;
        private List<int> _choices;

        private void Awake()
        {
            UiManagerToChange.ChoiceManager = this;
        }

        private void Start()
        {
            _choices = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
            UIManager.SetPlayerAvailableNumbers(_choices, true);
        }

        public void NumberChosen(int i)
        {
            Debug.Log("Player chose: " + i);
            _currentNumberValue = i; 
            var squares = new List<Vector2> {new Vector2(i-1, i-1) };
            UIManager.ShowSelectablePositions(squares);
        }

        public void SquareChosen(Vector2 index)
        {
            UIManager.SetNumberOnSquare(index, _currentNumberValue, _currentNumberValue, Color.blue);
            UIManager.ShowDamageOptions();

        }

        public void PickAttack(AttackType attackType)
        {
            _choices.Remove(_currentNumberValue);
            UIManager.SetPlayerAvailableNumbers(_choices, true);

        }
    }
}