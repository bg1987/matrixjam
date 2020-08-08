using System;
using System.Collections.Generic;
using MatrixJam.Team;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team4
{
    public class UIManager : MonoBehaviour
    {
        
        private static UIManager _instance;
        public NumberPool PlayerNumberPool;
        public NumberPool AiNumberPool;
        public GameObject DarkScreen;
        public GameObject Tooltips;

        public Button[] AttackButtons;
        public NumberButtonScript NumberPrefab;
        
        private void Awake()
        {
            _instance = this;
            DarkScreen.SetActive(false);
            Tooltips.SetActive(true);
            NumberButtonsManager.GenerateBoard(NumberPrefab);
            ShowDamageOptions(false);
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

        public static void ShowDamageOptions(bool interactable)
        {
            foreach (var attackButton in _instance.AttackButtons)
            {
                attackButton.interactable = interactable;

            }
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
            ShowDamageOptions(false);
            ChoiceManager.PickAttack(AttackType.Row);
        }
        public void PickAttackLine()
        {
            ShowDamageOptions(false);
            ChoiceManager.PickAttack(AttackType.Line);
        }
        public void PickAttackBox()
        {
            ShowDamageOptions(false);;
            ChoiceManager.PickAttack(AttackType.Box
            );
        }
    }
}