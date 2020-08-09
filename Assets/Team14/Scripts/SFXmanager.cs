using MatrixJam.Team14;
using UnityEngine;

namespace MatrixJam.Team
{
    public class SFXmanager : MonoBehaviour
    {
        [Header("Sound Effects")]
        [SerializeField] private GenericRandomSFX Jump;
        [SerializeField] private GenericRandomSFX Railways;
        [SerializeField] private GenericRandomSFX Honk;
        [SerializeField] private GenericRandomSFX AwakeBells;
        [SerializeField] private GenericRandomSFX Duck;
        [SerializeField] private GenericRandomSFX CatSqueals;
        
        public void PlaySFX(TrainMove move)
        {
            switch (move)
            {
                case TrainMove.Jump:
                    Jump.PlayRandom();
                    break;
                case TrainMove.Duck:
                    Duck.PlayRandom();
                    break;
                case TrainMove.Honk:
                    Honk.PlayRandom();
                    break;
            }
        }
    }
}
