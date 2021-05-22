using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Restart : MonoBehaviour
    {
        [SerializeField] Activator ingameActivator;

        [SerializeField] IngameMenuOverlay ingameMenuFG;
        [SerializeField] float restartDurationScale = 1;

        [SerializeField] float restartEffectAppearDuration = 1;
        [SerializeField] AnimationCurve restartEffectAppearCurve = AnimationCurve.EaseInOut(0,0,1,1);

        [SerializeField] float restartEffectInbetweenBlackScreenDuration = 1;

        [SerializeField] float restartEffectDisappearDuration = 1;
        [SerializeField] AnimationCurve restartEffectDisappearCurve = AnimationCurve.EaseInOut(0,0,1,1);
        private Coroutine restartGameRoutine;

        public void RestartGame()
        {
            if (restartGameRoutine != null)
                StopCoroutine(restartGameRoutine);

            if (restartDurationScale == 0)
            {
                ingameMenuFG.UncoverWholeScreen(0);
                RestartNow();
                return;
            }
            
            restartGameRoutine = StartCoroutine(RestartGameRoutine(restartDurationScale));
        }
        IEnumerator RestartGameRoutine(float restartDurationScale)
        {
            ingameMenuFG.Activate();
            
            ingameMenuFG.CoverWholeScreen(restartEffectAppearDuration * restartDurationScale, restartEffectAppearCurve);
            ingameMenuFG.SetInteractable(true);
            yield return new WaitForSeconds((restartEffectAppearDuration + restartEffectInbetweenBlackScreenDuration) * restartDurationScale);

            RestartNow();

            ingameMenuFG.UncoverWholeScreen(restartEffectDisappearDuration * restartDurationScale, restartEffectDisappearCurve);
            ingameMenuFG.SetInteractable(false);
            //ingameMenuFG.Deactivate((restartEffectDisappearDuration * restartDurationScale));

            restartGameRoutine = null;
        }
        void RestartNow()
        {
            ingameActivator.DeactivateImmediately();
            MatrixTraveler.Instance.ReTravelToCurrentGame();
            
        }
        
    }
}
