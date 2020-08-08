using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class BoardManager : MonoBehaviour
    {
        private int _size;
        private BoardData _boardData;
        private bool[,,] _possibleUnitsPerCoordinate;

        public BoardManager(int size, int initialRandomUnits = 30)
        {
            _size = size;

            InitPossibleUnitsPerCoord(size);
            InitBoardData(size);
            AddRandomUnits(initialRandomUnits);

        }
        
        public void ExecuteTurn(TurnObject turnObject)
        {
            var unit = turnObject.ChosenUnit;
            var player = unit.Owner;
            var attackDirection = turnObject.AttackDirection;
            int points = 0;
            switch (attackDirection)
            {
                case AttackDirection.colum:
                    points = GetColumPoints(player, unit);
                    break;
                case AttackDirection.row:
                    points = GetRowPoints(player, unit);
                    break;
                case AttackDirection.square:
                    points = GetSquarePoints(player, unit);
                    break;
            }

            //TODO PlaceUnit(unit, position);

            EventManager.Singleton.OnTurnOver();
        }

        private int GetSquarePoints(Player player, Unit unit)
        {
            var ans = 0;
            //TODO get square units
            //TODO sum square units
            return ans;
        }

        private int GetRowPoints(Player player, Unit unit)
        {            
            //TODO Implement this
            int ans = 0;
            for (int i = 0; i < _size; i++)
            {
                //var unitToCheck = _boardData._boardData[i, ] //I AM HERE
            
            }

            return ans;
        }

        private int GetColumPoints(Player player, Unit unit)
        {
            //TODO Implement this
            int ans = 0;
            for (int i = 0; i < _size; i++)
            {
                //var unitToCheck = _boardData._boardData[i, ] //I AM HERE

            }

            return ans;
        }

        public TurnData GetPlayerTurnData(Player _player)
        {
            var boardData = new BoardData(_boardData._boardData);
            var possiblePositionByUnit = GetPlayersUnitsOptions(_player);

            return new TurnData(boardData, possiblePositionByUnit);
        }

        private List<Position> GetUnitsLegalPositions(Unit unit)
        {
            var unitValueOptions = GetUnitOptions(unit);
            var unitOptions = new List<Position>();
            for (int i = 0; i < unitValueOptions.Count; i++)
            {
                var pos = new Position(unitValueOptions[i] / 10, unitValueOptions[i] % 10);
                unitOptions.Add(pos);
            }

            return unitOptions;
        }

        private Dictionary<Unit, List<Position>> GetPlayersUnitsOptions(Player player)
        {
            var options = new Dictionary<Unit, List<Position>>();

            for (int i = 0; i < player.MyUnits.Count; i++)
            {
                var unit = player.MyUnits[i];
                var unitOptions = GetUnitsLegalPositions(unit);

                options.Add(unit, unitOptions);
            }

            return options;
        }

        private void AddRandomUnits(int initialRandomUnits)
        {
            for ( ; initialRandomUnits > 0; initialRandomUnits-- )
            {
                AddRandomUnit();
            }
        }

        private void AddRandomUnit()
        {
            Debug.Log("BoardManager:AddRandomUnit");
            do
            {
                int value = UnityEngine.Random.Range(0, 8);
                var possibeOptions = GetUnitOptions(value);
                if (possibeOptions.Count > 0)
                {
                    var randomIndex = UnityEngine.Random.Range(0, possibeOptions.Count);
                    var indexedCoordinate = possibeOptions[randomIndex];
                    int x = indexedCoordinate / 10;
                    int y = indexedCoordinate % 10;
                    int realValue = value + 1;

                    Unit unit = new Unit(null, realValue);
                    Position position = new Position(x, y);
                    if (PlaceUnit(unit, position))
                    {
                        return;
                    }
                }
            }
            while (true);      
        }

        private List<int> GetUnitOptions(Unit unit)
        {
            return GetUnitOptions(unit.Value - 1);
        }

        private List<int> GetUnitOptions(int value)
        {
            List<int> ans = new List<int>();

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if ( _possibleUnitsPerCoordinate[i,j,value] )
                    {
                        ans.Add(i * 10 + j);
                    }
                }
            }       
            return ans;
        }

        private void InitBoardData(int size)
        {
            _boardData = new BoardData(size, size);
        }

        private void InitPossibleUnitsPerCoord(int size)
        {
            _possibleUnitsPerCoordinate = new bool[size, size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < 9; k++)
                        _possibleUnitsPerCoordinate[i, j, k] = true;
                }
            }
        }

        private Boolean PlaceUnit(Unit unit, Position position)
        {
            var x = position.GetX();
            var y = position.GetY();
            return PlaceUnit(unit, x, y);
        }

        private Boolean PlaceUnit(Unit unit, int x, int y)
        {
            Debug.Log("BoardManager:PlaceUnit");
            var unitIndex = unit.Value - 1;
            Debug.Log("Attempting PlaceUnit position, value:" + x.ToString() + " / " + y.ToString() + ", " + unit.Value.ToString());
            if (!_possibleUnitsPerCoordinate[x, y, unitIndex])
            {
                Debug.Log("Position is taken! Aborting PlaceUnit!");
                return false;
            }

            if (_boardData.PlaceUnit(x, y, unit))
            {
                PlaceNumberInUI(unit, x, y);
                SetUnitIllegal(x, y, unitIndex);
                Debug.Log("PlaceUnit succeeded - position, value:" + x.ToString() + " / " + y.ToString() + ", " + unit.Value.ToString());

                return true;
            }

            return false;
        }
        
        private static void PlaceNumberInUI(Unit unit, int x, int y)
        {
            
            var playerColor = Color.black;
            if (unit.Owner != null)
            {
                playerColor =unit.Owner.Color(); 
            }
            UIManager.SetNumberOnSquare(new Vector2(x, y), unit.Value, unit.Value, playerColor);
        }
        
        private void SetUnitIllegal(int x, int y, int unitIndex)
        {
            IllegalizeLines(x, y, unitIndex);
            IllegalizeSquare(x, y, unitIndex);
        }

        public void IllegalizeSquare(int x, int y, int unitIndex)
        {
            for (int i = 0; i < _size; i++)
            {
                _possibleUnitsPerCoordinate[i, y, unitIndex] = false;
                _possibleUnitsPerCoordinate[x, i, unitIndex] = false;
            }

        }

        public void IllegalizeLines(int x, int y, int unitIndex)
        {
            var square = new Square(x, y);

            for (int i = square.startX; i < square.startX + 3; i++)
            {
                for (int j = square.startY; j < square.startY + 3; j++)
                {
                    _possibleUnitsPerCoordinate[i, j, unitIndex] = false;
                }
            }
        }
    }

    public class Square
    {
        public int startX;
        public int startY;

        public Square(int x, int y)
        {
            int _startX = x / 3;
            _startX *= 3;

            int _startY = y / 3;
            _startY *= 3;
        }

    }

}