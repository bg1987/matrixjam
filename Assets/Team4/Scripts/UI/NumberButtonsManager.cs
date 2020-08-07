using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class NumberButtonsManager
    {
        public static int BOARD_SIZE = 9;
        private static NumberButtonScript[][] _allButtons = new NumberButtonScript[BOARD_SIZE][];
        public static void GenerateBoard(NumberButtonScript numberPrefab)
        {
            var squareWidth = SquareWidth(numberPrefab);
            var originalPosition = numberPrefab.transform.localPosition;
            
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                _allButtons[i] = new NumberButtonScript[BOARD_SIZE];
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    Vector3 position = originalPosition + new Vector3(i * squareWidth, -j * squareWidth, 0);
                    var clone = GameObject.Instantiate(numberPrefab, numberPrefab.transform.parent);
                    clone.transform.localPosition = position;
                    clone.Index = new Vector2(i,j);
                    _allButtons[i][j] = clone;
                    clone.Clear();
                }
            }
            numberPrefab.gameObject.SetActive(false);
        }

        public static float SquareWidth(NumberButtonScript numberPrefab)
        {
            var renderer = numberPrefab.GetComponent<RectTransform>();
            var squareWidth = renderer.rect.width;
            return squareWidth;
        }

        public static void SetSquaresForPuttingNumberOn(List<Vector2> squares)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    _allButtons[i][j].MakeSelectable(IsInSelectableSquares(i, j, squares));
                }
            }
                
        }

        private static bool IsInSelectableSquares(int i, int j, List<Vector2> squares)
        {
            foreach (var square in squares)
            {
                if (square.x == i && square.y == j)
                {
                    return true;

                }
            }

            return false;
        }

        public static void MakeAllUnselectable()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    _allButtons[i][j].MakeSelectable(false);
                }
            }
        }

        public static void SetNumberOnSquare(Vector2 index, int originalValue, int currentValue, Color playerColor)
        {
            _allButtons[(int) index.x][(int) index.y].SetValue(originalValue, currentValue, playerColor);
        }
    }
}