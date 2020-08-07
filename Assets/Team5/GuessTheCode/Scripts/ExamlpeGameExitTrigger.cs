using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team5
{
    public class ExamlpeGameExitTrigger : MonoBehaviour
    {
        [SerializeField] private int exitNum;

        [SerializeField] private UnityEvent exitEvent;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger Enter" + exitNum);
            exitEvent.Invoke();
        }

    }
}