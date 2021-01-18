using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta
{
    public class ContinueButton : MonoBehaviour
    {
        [SerializeField] Button button;

        // Start is called before the first frame update
        void Start()
        {
            button.interactable = MatrixTraveler.Instance.travelData.IsPossibleToLoadFromDisk();
        }
    }
}
