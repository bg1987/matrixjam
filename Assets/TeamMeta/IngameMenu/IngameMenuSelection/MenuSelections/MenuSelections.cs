using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class MenuSelections : MonoBehaviour
    {
        [SerializeField] List<Selection> selections;
        public List<Selection> Selections { get => selections; }
    }
}
