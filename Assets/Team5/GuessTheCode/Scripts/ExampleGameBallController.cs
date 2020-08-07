using UnityEngine;

namespace MatrixJam.Team5
{
    public class ExampleGameBallController : MonoBehaviour
    {

        [SerializeField] private float Speed = 1;

        [SerializeField] private Vector3 inputVector;

        // Update is called once per frame
        void Update()
        {
            inputVector.x = Input.GetAxis("Horizontal");
            inputVector.z = Input.GetAxis("Vertical");
            gameObject.GetComponent<Rigidbody>().AddForce(inputVector * Speed);
        }
    }
}
