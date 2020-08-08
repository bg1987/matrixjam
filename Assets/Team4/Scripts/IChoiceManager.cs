using MatrixJam.Team4;
using UnityEngine;

namespace MatrixJam.Team
{
    public interface IChoiceManager
    {

        void StartTurn(Player player);
        void NumberChosen(int value);
        void SquareChosen(Vector2 index);
        void PickAttack(AttackDirection attackType);
    }

}