using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Skip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI skipCountText;
        [SerializeField] Selection skipSelection;

        [SerializeField] int skipsRemaining = 2;
        private void Awake()
        {
            skipCountText.text = skipsRemaining + "";
        }
        public void SkipToNextGame()
        {
            skipsRemaining--;
            skipCountText.text = skipsRemaining+"";

            if(skipsRemaining == 0)
            {
                skipSelection.SetHidden(true);
                skipSelection.SetInteractable(false);

                //The below commented out line isn't necessary since menu disappearance will be handled at skip time
                //skipSelection.Disappear(0.5f, (selection)=> selection.gameObject.SetActive(false));
            }

            bool success = MatrixTraveler.Instance.TryWarpToRandomUnvisitedGame(); //Implement SkipToRandomGame and decide if it should be an unvisited game or just a different game
            if (!success)
                MatrixTraveler.Instance.WarpToRandomGame();
        }
    }
}
