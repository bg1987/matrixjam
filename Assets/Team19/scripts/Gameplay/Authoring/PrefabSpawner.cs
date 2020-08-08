using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Authoring
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _spawnedPrefab;

        [SerializeField]
        private float _spawnIntervalSeconds;

        private float _lastSpawnTime;

        private void Start()
        {
            _lastSpawnTime = Time.time;
        }

        private void Update()
        {   
            if (Time.time - _lastSpawnTime > _spawnIntervalSeconds)
            {
                Instantiate(_spawnedPrefab, transform.position, transform.rotation);
                _lastSpawnTime = Time.time;
            }
        }
    }
}
