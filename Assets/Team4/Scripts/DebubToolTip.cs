using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class DebubToolTip : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(RandomTooltips());
        }

        private IEnumerator RandomTooltips()
        {
            yield return new WaitForSeconds(1);
            UIManager.ShowMessage("Some tooltip above board", MessageLocation.AboveBoard);
            yield return new WaitForSeconds(3);
            UIManager.ShowMessage("Another tooltip above board", MessageLocation.AboveBoard);
            yield return new WaitForSeconds(3);
            UIManager.ShowMessage("Tooltip about the score", MessageLocation.NextToScore);
            yield return new WaitForSeconds(3);
            UIManager.ShowMessage("Tooltip about the player pool", MessageLocation.PlayerPool);
            yield return new WaitForSeconds(3);
            UIManager.ShowMessage("Tooltip about the Attack options", MessageLocation.AttackOptions);
            yield return new WaitForSeconds(3);
            UIManager.ShowMessage("Tooltip about the wait queue", MessageLocation.PlayerWaitQueue);
            yield return new WaitForSeconds(3);
            UIManager.ShowMessage("Tooltip about the enemy pool", MessageLocation.EnemyPool);
            yield return new WaitForSeconds(3);
            UIManager.ShowMessage("And another tooltip above board", MessageLocation.AboveBoard);
            yield return new WaitForSeconds(3);
            UIManager.HideMessage();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
