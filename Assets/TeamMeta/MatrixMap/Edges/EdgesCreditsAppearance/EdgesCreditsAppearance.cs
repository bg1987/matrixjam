using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgesCreditsAppearance : MonoBehaviour
    {
        [SerializeField] private float delayBetweenEdgesHistoryEntriesSequenceAppear = 0.5f;
        [SerializeField] private float edgeSequenceAppearDuration = 0.5f;
        [SerializeField] private float sequenceAppearDelay = 1.5f;
        [SerializeField] private EdgeRevisitEffect creditsEffect;
        [SerializeField] private EdgeFirstVisitEffect edgeFirstVisitEffect;
        public void Appear(List<MatrixEdgeData> edgesSequence, Edges edgesController)
        {
            StartCoroutine(AppearRoutine(edgesSequence, edgesController));
        }
        IEnumerator AppearRoutine(List<MatrixEdgeData> edgesSequence, Edges edgesController)
        {
            yield return new WaitForSeconds(sequenceAppearDelay);
            SortedSet<int> alreadyAppearedEdgesIndexes = new SortedSet<int>();

            List<int> edgesIdsSequence = new List<int>();
            foreach (var edgeData in edgesSequence)
            {
                edgesIdsSequence.Add(edgesController.FindEdgeIndexByEdgeData(edgeData));
            }


            List<Edge> edges = edgesController.GetEdges();
            for (int i = 0; i < edgesIdsSequence.Count - 1; i++)
            {
                if (edgesIdsSequence[i] == -1)
                {
                    yield return new WaitForSeconds(delayBetweenEdgesHistoryEntriesSequenceAppear);
                    continue;
                }
                var edge = edges[edgesIdsSequence[i]];
                if (!alreadyAppearedEdgesIndexes.Contains(edge.index))
                {
                    edge.gameObject.SetActive(true);
                    edge.Appear(edgeSequenceAppearDuration, 0);
                    alreadyAppearedEdgesIndexes.Add(edge.index);
                    edgeFirstVisitEffect.Play(edge, 0);
                }

                creditsEffect.SetTravelDuration(edgeSequenceAppearDuration);
                creditsEffect.SetEndEdgeColorHeight(0.0f);
                creditsEffect.SetSparksStopAtEnd(true);
                creditsEffect.Play(edge, 0.00f);

                yield return new WaitForSeconds(delayBetweenEdgesHistoryEntriesSequenceAppear);
            }
        }
    }
}
