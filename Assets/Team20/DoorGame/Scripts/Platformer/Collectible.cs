using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class Collectible : MonoBehaviour
    {
        private enum CollectibleType
        {
            Coin,
            Type2,
            Type3
        }
        [SerializeField] private CollectibleType _collectibleType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.GetComponent<Player>();
                if (_collectibleType == 0 && player != null)
                {
                    player.AddCoins(1);
                }
                Destroy(gameObject);
            }
        }
    }
}
