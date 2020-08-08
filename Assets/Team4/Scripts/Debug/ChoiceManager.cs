using System;
using System.Collections.Generic;
using System.Linq;
using MatrixJam.Team;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class ChoiceManager : IChoiceManager
    {
        private BoardManager _boardManager;
        private TurnData _playerTurnData;
        private Unit _selectedUnit;
        private Position _selectedPosition;

        public ChoiceManager(BoardManager boardManager)
        {
            _boardManager = boardManager;
        }

        public void StartTurn(Player player, TurnData turnData)
        {
            ClearData();
            _playerTurnData = turnData;
            List<int> choices = new List<int>();
            foreach (var unit in _playerTurnData._positionOptions.Keys)
            {
                choices.Add(unit.Value);
            }
            UIManager.SetPlayerAvailableNumbers(choices, player.IsHuman());

            if (!player.IsHuman())
            {
                //TODO trigger AI
            }

        }

        private void ClearData()
        {
            UIManager.ShowSelectablePositions(new Vector2[0]); //clear
            _selectedUnit = null;
            _selectedPosition = null;
        }

        public void PickNumber(int value)
        {
            Debug.Log("Player chose: " + value);
            foreach (var unit in _playerTurnData._positionOptions.Keys)
            {
                if (unit.Value == value)
                {
                    UpdateSelectedUnit(unit);
                    return;
                }
            }
            throw new Exception("Could not find number");
        }

        private void UpdateSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;
            if (unit.Owner.IsHuman())
            {
                var positionOption = _playerTurnData._positionOptions[unit];
                Vector2[] squares = GetSquarsFromAllowedPositions(positionOption);
                UIManager.ShowSelectablePositions(squares);
            }
        }

        private Vector2[] GetSquarsFromAllowedPositions(List<Position> positionOption)
        {
            Vector2[] squares = positionOption.Select(x => new Vector2(x.GetX(), x.GetY())).ToArray();
            return squares;
        }

        public void PickSquare(Vector2 index)
        {
            var positionOption = _playerTurnData._positionOptions[_selectedUnit];
            var position = positionOption.Find(p => p.GetX() == index.x && p.GetY() == index.y);
            if (position != null)
            {
                _selectedPosition = position;
                UIManager.SetNumberOnSquare(index, _selectedUnit.Value, _selectedUnit.Value, _selectedUnit.Owner.Color);
                UIManager.ShowDamageOptions(_selectedUnit.Owner.IsHuman());
            }

        }

        public void PickAttack(AttackDirection attackType)
        {
            var turnObject = new TurnObject();
            turnObject.ChosenUnit = _selectedUnit;
            turnObject.ChosenUnit.Position = _selectedPosition;
            turnObject.AttackDirection = attackType;
            _boardManager.ExecuteTurn(turnObject);
        }
    }
}