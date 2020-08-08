using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    public class BeaconManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _beacons;

        private int _nextBeaconIndex = 0;

        [SerializeField]
        private void Awake()
        {
            foreach (GameObject beacon in _beacons)
            {
                beacon.SetActive(false);
            }

            LevelManager.LevelPassed += OnLevelPassed;
        }

        private void OnLevelPassed()
        {
            _beacons[_nextBeaconIndex].SetActive(true);
            _nextBeaconIndex ++;
        }
    }
}
