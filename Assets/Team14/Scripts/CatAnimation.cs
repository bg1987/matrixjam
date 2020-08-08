using System.Linq;
using UnityEngine;

namespace MatrixJam.Team
{
    public class CatAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
        GameObject cat;
        public GameObject[] cats;
        int index;
        public int SPEED;
        public UnityEngine.Events.UnityEvent jump;

        void Start()
        {
            cats = transform
                .Cast<Transform>()
                .Select(trans => trans.gameObject)
                .ToArray();
            index = 0;
            
        }

        // Update is called once per frame
        void Update()
        {
            Jump();
        }
        //[ContextMenu("Jump")]
        public void Jump()
        {
            if (index < (cats.Length - 1) * SPEED)
            {
                cats[index / SPEED].SetActive(false);
                cats[(index + 1) / SPEED].SetActive(true);
                index++;
            }
            else if (index == (cats.Length - 1) * SPEED)
            {
                cats[index / SPEED].SetActive(false);
                index++;
            }
        }
    }
}
