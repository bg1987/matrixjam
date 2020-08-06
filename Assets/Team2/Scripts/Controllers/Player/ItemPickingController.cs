using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class ItemPickingController : MonoBehaviour
    {
        private Pickable pickable;

        private void Update()
        {
            Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsPickablePicked())
                {
                    DropPickable();
                }
                else
                {
                    Pickup(mousePositionInWorld);
                }
            }

            if (IsPickablePicked())
            {
                MovePickable(mousePositionInWorld);
            }
        }

        public void Pickup(Vector2 mousePositionInWorld)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(mousePositionInWorld, Vector2.zero, 0f);
            if (raycastHit.collider != null)
            {
                Pickable pickable;
                if (raycastHit.collider.TryGetComponent(out pickable))
                {
                    this.pickable = pickable;
                }
            }
        }

        private bool IsPickablePicked()
        {
            return pickable != null;
        }

        private void DropPickable()
        {
            pickable = null;
        }

        private void MovePickable(Vector2 mousePositionInWorld)
        {
            pickable.transform.position = mousePositionInWorld;
        }
    }
}
