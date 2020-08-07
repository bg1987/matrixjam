using UnityEngine;

namespace MatrixJam.Team
{
    public interface IChoiceManager
    {
        void NumberChosen(int i);
        void SquareChosen(Vector2 index);
        void PickAttack(AttackType attackType);
    }

    public enum AttackType
    {
        Row,
        Line,
        Box
        
    }
}