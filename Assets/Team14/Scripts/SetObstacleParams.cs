using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team14
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(SetObstacleParams))]
    public class SetTriggerOffsetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = target as SetObstacleParams;
            if (GUILayout.Button("DO IT"))
            {
                script.SetParams();
            }
        }
    }
#endif
    
    public class SetObstacleParams : MonoBehaviour
    {
        [SerializeField] private GameObject[] obstacleTypes;
        [SerializeField] private BoxCollider trigger;
        [SerializeField] private Transform obstacleHolder;
        
        [Header("Sizes")]
        [SerializeField] private float zPerBeat;
        [SerializeField] private float triggerBeatWidth;
        [SerializeField] private float obstacleBeatOffset;

        public void SetParams()
        {
            AdjustTrigger();
            
            AdjustObstacleHolder();
            
            // Doesn't work (Delete in prefab instance)
            // DestroyHolderChildren();
            
            CreateRandomObstacle();
        }

        private void DestroyHolderChildren()
        {
            // TODO: destory doesn't work cause its a prefab (how to do this in a prefab instance)
            foreach (var child in obstacleHolder.transform.Cast<Transform>().ToArray())
            {
                DestroyImmediate(child.gameObject);
            }
        }

        public void AdjustObstacleHolder()
        {
            if (!obstacleHolder) return;
            var delta = zPerBeat * obstacleBeatOffset;
            var oldPos = obstacleHolder.transform.localPosition;
            var pos = new Vector3(oldPos.x, oldPos.y, delta);

            obstacleHolder.transform.localPosition = pos;
        }

        public void AdjustTrigger()
        {
            var oldSize = trigger.size;
            var triggerWidth = zPerBeat * triggerBeatWidth;
            trigger.size = new Vector3(oldSize.x, oldSize.y, triggerWidth);
        }

        private void CreateRandomObstacle()
        {
            if (obstacleTypes.Length == 0)
            {
                Debug.LogError($"No Random Obstacles Configured! ({name})");
                return;
            }

            var randObstacle = obstacleTypes[Random.Range(0, obstacleTypes.Length)];
            
            var go = (GameObject) PrefabUtility.InstantiatePrefab(randObstacle, obstacleHolder);
            go.transform.localPosition = Vector3.zero;
        }
    }
}
