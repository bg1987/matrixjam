using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    public enum ETimeline
    {
        BRIGHTEST_TIMELINE,
        DARKEST_TIMELINE,
    }

    [System.Serializable]
    public class ModifiedContentManager
    {
        private static Vector3 SPAWN_POINT = Vector3.zero;

        [SerializeField]
        private Transform _modifiableRoot;

        [SerializeField]
        private GameObject[] _modifiedContentPrefabs;
        
        private GameObject _activeContent;

        private ETimeline _activeTimeline = ETimeline.DARKEST_TIMELINE;

        private int _activeLevelIndex;

        public void InitializeByEntrance(int entrance_num, int max_progress)
        {

            switch (entrance_num)
            {
                case 0:
                    _activeTimeline = ETimeline.DARKEST_TIMELINE;
                    _activeLevelIndex = 0;
                    break;

                case 1:
                    _activeTimeline = ETimeline.BRIGHTEST_TIMELINE;
                    _activeLevelIndex = max_progress;
                    break;
            }
        }

        public void ModifyContentByProgress(int progress)
        {
            GameObject.Destroy(_activeContent);

            AdvanceActiveTimeline(progress);

            GameObject activeContentPrefab = _modifiedContentPrefabs[_activeLevelIndex];
            _activeContent = GameObject.Instantiate(activeContentPrefab, _modifiableRoot);
        }

        private void AdvanceActiveTimeline(int progress)
        {
            switch (_activeTimeline)
            {
                case ETimeline.DARKEST_TIMELINE:
                    _activeLevelIndex ++;
                    break;

                case ETimeline.BRIGHTEST_TIMELINE:
                    _activeLevelIndex --;
                    break;
            }
        }
    }
}
