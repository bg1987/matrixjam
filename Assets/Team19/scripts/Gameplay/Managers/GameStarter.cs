using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    public class GameStarter : StartHelper
    {
        [SerializeField]
        private GameObject _playerPrefab;

        public override void StartHelp(int num_ent)
        {
            Instantiate(_playerPrefab, transform.position, transform.rotation);
        }
    }
}
