using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class SelectorTutorial : MonoBehaviour
    {
        [SerializeField] TutorialStep nodeTutorialStep;
        [SerializeField] TutorialStep edgeTutorialStep;
        [SerializeField] int minimumVisitedGamesToAllowActivation = 1;
        bool isActive = false;
        
        [Header("Step Appear")]
        [SerializeField] float stepAppearDuration = 0.2f;
        [SerializeField] float stepAppearCharacterDuration = 0.75f;
        [SerializeField] float stepAppearDelay = 0.5f;
        [Header("Step Disappear")]
        [SerializeField] float stepDisappearDuration = 0.2f;
        [SerializeField] float stepDisappearCharacterDuration = 0.5f;
        [Header("Step Complete")]
        [SerializeField] float stepCompleteDuration = 0.02f;
        [SerializeField] float stepCompleteCharacterDuration = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            Deactivate();

        }
        public void NodeSelected(Node node)
        {
            if (!nodeTutorialStep.isCompleted)
                nodeTutorialStep.Complete(stepCompleteDuration, stepCompleteCharacterDuration);

            if (edgeTutorialStep.isCompleted)
                return;
            if (node.startPortActiveEdges.Count == 0)
            {
                NodeDeselected();
                return;
            }
            if (edgeTutorialStep.isInProgress)
                return;
            if (isActive)
            {
                edgeTutorialStep.Appear(stepAppearDuration, stepAppearCharacterDuration, stepAppearDelay);
            }
        }
        public void EdgeHovered()
        {
            if (!edgeTutorialStep.isCompleted)
                edgeTutorialStep.Complete(stepCompleteDuration, stepCompleteCharacterDuration);
        }
        public void NodeDeselected()
        {
            if (edgeTutorialStep.isInProgress)
            {
                edgeTutorialStep.Disappear(stepDisappearDuration, stepDisappearCharacterDuration);
            }
        }
        public void Deactivate()
        {
            isActive = false;

            nodeTutorialStep.DisappearImmediately();
            edgeTutorialStep.DisappearImmediately();

            nodeTutorialStep.gameObject.SetActive(false);
            edgeTutorialStep.gameObject.SetActive(false);
        }
        public void Activate()
        {
            int visitedGamesCount = MatrixTraveler.Instance.travelData.GetVisitedGamesCount();
            if (visitedGamesCount < minimumVisitedGamesToAllowActivation)
                return;

            isActive = true;

            nodeTutorialStep.gameObject.SetActive(true);
            edgeTutorialStep.gameObject.SetActive(true);

            if (!nodeTutorialStep.isCompleted)
                nodeTutorialStep.Appear(stepAppearDuration, stepAppearCharacterDuration, 0);
        }
    }
}
