using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team2
{
    public class BroniReceiverController : MonoBehaviour
    {
        [SerializeField] private float deactivationTime;
        [SerializeField] private UnityEvent onActived;
        [SerializeField] private UnityEvent onDeactived;

        private bool isActive = false;
        private float deactivationTimer;

        void Start()
        {
            deactivationTimer = deactivationTime;
        }

        void Update()
        {
            // Deactivation countdown
            if (!isActive) return;
            deactivationTimer -= Time.deltaTime;
            if (deactivationTimer <= 0)
            {
                Deactivate();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            deactivationTimer = deactivationTime;
            if (other.TryGetComponent(out BulletController bulletController))
            {
                // TODO: Activate unity particle and/or change sprite.
                if (!isActive)
                {
                    Activate();
                }
                Destroy(bulletController.gameObject);
            }
        }

        void Activate()
        {
            Debug.Log("Activated");
            isActive = true;
            onActived.Invoke();
        }

        void Deactivate()
        {
            Debug.Log("Deactivated");
            isActive = false;
            onDeactived.Invoke();
        }
    }
}
