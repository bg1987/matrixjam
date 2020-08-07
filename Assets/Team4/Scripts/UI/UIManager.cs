using System;
using System.Collections.Generic;
using MatrixJam.Team;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class UIManager : MonoBehaviour
    {
        
        private static UIManager _instance;
        public NumberPool PlayerNumberPool;
        public NumberPool AiNumberPool;
        public GameObject DarkScreen;
        public GameObject Tooltips;

        public GameObject AttackPanel;
        public NumberButtonScript NumberPrefab;
        
        private void Awake()
        {
            _instance = this;
            DarkScreen.SetActive(false);
            Tooltips.SetActive(true);
            NumberButtonsManager.GenerateBoard(NumberPrefab);
        }

        public static void ShowMessage(string message, MessageLocation location)
        {
            TooltipManager.ShowMessage(message, location);
        }

        public static void HideMessage()
        {
            TooltipManager.HideMessage();
        }

        public static void ShowDarkScreen()
        {
            _instance.DarkScreen.SetActive(true);
        }
        
        public static void HideDarkScreen()
        {
            _instance.DarkScreen.SetActive(false);
        }
        
        public static void NumberChosen(int i)
        {
            _instance.ChoiceManager.NumberChosen(i);
        }

        public IChoiceManager ChoiceManager { get; set; }
        

        public static void SquareChosen(Vector2 index)
        {
            NumberButtonsManager.MakeAllUnselectable();
            _instance.PlayerNumberPool.MakeNoneSelectable();
            _instance.ChoiceManager.SquareChosen(index);
        }

        public static void ShowSelectablePositions(List<Vector2> squares)
        {
            NumberButtonsManager.SetSquaresForPuttingNumberOn(squares);
        }

        public static void ShowDamageOptions()
        {
            _instance.AttackPanel.SetActive(true);
        }

        public static void SetNumberOnSquare(Vector2 index, int originalValue, int currentValue, Color playerColor)
        {
            NumberButtonsManager.SetNumberOnSquare(index, originalValue, currentValue, playerColor);
        }

        public static void SetPlayerAvailableNumbers(List<int> choices, bool humanPlayer)
        {
            if (humanPlayer)
            {
                _instance.PlayerNumberPool.SetAvailableNumbers(choices);
            }
            else
            {
                _instance.AiNumberPool.SetAvailableNumbers(choices);
            }
        }

        public void PickAttackRow()
        {
            AttackPanel.SetActive(false);
            ChoiceManager.PickAttack(AttackType.Row);
        }
        public void PickAttackLine()
        {
            AttackPanel.SetActive(false);
            ChoiceManager.PickAttack(AttackType.Line);
        }
        public void PickAttackBox()
        {
            AttackPanel.SetActive(false);
            ChoiceManager.PickAttack(AttackType.Box
            );
        }
    }
}