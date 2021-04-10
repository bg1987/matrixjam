using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    [CustomEditor(typeof(EdgeRevisitEffect))]
    public class EdgeRevisitEffectEditor : Editor
    {
        EdgeRevisitEffect effect;
        Edge edge;
        [SerializeField, Range(0, 1)] float effectTime = 0;
        bool shouldAnimate = true;
        private void OnEnable()
        {
            effect = target as EdgeRevisitEffect;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var headerSkin = GUI.skin.label;
            headerSkin.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Editor", headerSkin);
            edge = EditorGUILayout.ObjectField(edge, typeof(Edge), allowSceneObjects:true) as Edge;
            if (edge == null)
                return;
            //if(GUILayout.Button("Button"));
            shouldAnimate = EditorGUILayout.Toggle("Animate", shouldAnimate);
            
            if (Application.isPlaying == false)
            {
                EditorGUILayout.LabelField("Can Only Animate In Play Mode");
                return;
            }
            if(GUILayout.Button("Play Effect")){
                effect.Play(edge, 0);
            }
            if (shouldAnimate == false)
            {
                return;
            }
            float newEffectTime = EditorGUILayout.Slider("Effect Progress",effectTime, 0, 1);

            if (effectTime != newEffectTime)
            {
                effectTime = newEffectTime;

                effect.Execute(effectTime, edge.Material);
            }
        }
    }
}
