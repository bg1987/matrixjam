using MatrixJam.Team14;
using UnityEngine;

namespace MatrixJam.Team
{
    public class SFXmanager : MonoBehaviour
    {
        [Header("Sound Effects")]
        [SerializeField] private GenericRandomSFX Jump;
        [SerializeField] private GenericRandomSFX Duck;
        [SerializeField] private GenericRandomSFX Honk;
        [SerializeField] private GenericRandomSFX DuckEnd;

        [SerializeField] private GenericRandomSFX CatSqueals;

        [SerializeField] private GenericRandomSFX Railways;
        [SerializeField] private GenericRandomSFX AwakeBells;
        [SerializeField] private GenericRandomSFX TunnelBump;

        private void Update()
        {

        }


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
                case TrainMove.DuckEnd:
                    DuckEnd.PlayRandom();
                    break;
            }
        }
    }

    //Reference to game BPM --> Activating Railway clips according to BPM/BPM change //// Trigger points on map to change railway clip
}
